using System.Security.Claims;
using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeestBeest.Web.Controllers;

[Route("/shop")]
public class OrderController : Controller
{
    private readonly ProductService productService;
    private readonly BasketService basketService;
    private readonly OrderService orderService;
    private readonly AccountService accountService;
    private readonly ILogger<OrderController> logger;

    public OrderController(ProductService productService, BasketService basketService, OrderService orderService, AccountService accountService, ILogger<OrderController> logger)
    {
        this.productService = productService;
        this.basketService = basketService;
        this.orderService = orderService;
        this.accountService = accountService;
        this.logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? message = "")
    {
        logger.LogInformation("Index action called with message: {Message}", message);
        ViewBag.Message = message;
        basketService.ClearBasket();

        var model = new OrderViewModel
        {
            Date = DateTime.Now 
        };

        return View(model);
    }

    [HttpPost("index-post")]
    public IActionResult IndexPost(DateOnly date)
    {
        logger.LogInformation("IndexPost action called with date: {Date}", date);
        if (date < DateOnly.FromDateTime(DateTime.Now))
        {
            logger.LogWarning("Date is in the past: {Date}", date);
            return View("Index", new OrderViewModel { Date = DateTime.Now });
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }

    [HttpGet("shop")]
    public IActionResult Shop(DateOnly date, List<ProductType>? selectedTypes, string? result, bool check = true)
    {
        var products = productService.GetProducts(date, selectedTypes);
        var basketProducts = basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.InBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        var model = new OrderViewModel()
        {
            OrderFor = date,
            Check = check,
            Result = result,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = selectedTypes ?? new List<ProductType>(),
                BasketCount = basketService.GetBasketItemCount()
            }
        };
        return View(model);
    }

    [HttpGet("contact-info")]
    public IActionResult ContactInfo(DateOnly date, OrderViewModel? OVmodel = null)
    {
        var model = new OrderViewModel();

        if (OVmodel != null)
        {
            model.Name = OVmodel.Name;
            model.Email = OVmodel.Email;
            model.ZipCode = OVmodel.ZipCode;
            model.HouseNumber = OVmodel.HouseNumber;
            model.PhoneNumber = OVmodel.PhoneNumber;
            model.TotalPrice = OVmodel.TotalPrice;
            model.DiscountAmount = OVmodel.DiscountAmount;
        }
        model.OrderFor = date;
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
            SelectedTypes = new List<ProductType>(),
            BasketCount = basketService.GetBasketItemCount()
        };
        return View(model);
    }

    [HttpPost("contact-info-post")]
    public IActionResult ContaxtInfoPost(OrderViewModel model, bool skip)
    {
        if (skip)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId != null)
            {
                var parsedId = int.Parse(userId);
                var account = accountService.GetUserById(parsedId);
                model.Name = account.Name;
                model.Email = account.Email;
                model.ZipCode = account.ZipCode;
                model.OrderFor = model.OrderFor;
                model.HouseNumber = account.HouseNumber;
                model.PhoneNumber = account.PhoneNumber;

                ModelState.Clear();
                TryValidateModel(model);
            }
        }

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };

        return ModelState.IsValid ? RedirectToAction("Confirm", model) : RedirectToAction("ContactInfo", new { date = model.OrderFor.ToString("yyyy-MM-dd"), OVmodel = model });
    }

    [HttpGet("confirm")]
    public IActionResult Confirm(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel     
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;
        model.DiscountAmount = model.TotalPrice * (100 - orderService.DiscountCheckRules(parsedId, model.ToDto())) / 100;

        return View(model);
    }

    [HttpPost("confirm-post")]
    public IActionResult ConfirmPost(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;

        orderService.CreateOrder(model.ToDto(), parsedId);
        basketService.ClearBasket();

        return RedirectToAction("Index", new { message = "Order created successfully, you may have paid for more products than expected :)" });
    }

    [HttpPost("add-to-basket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parsedId = userId != null ? int.Parse(userId) : (int?)null;
            var isValid = basketService.AddToBasket(product, parsedId);
            if (isValid)
            {
                product.InBasket = true; 
            }
            else
            {
                return RedirectToAction("Shop", new { date, check = isValid });
            }
        }
        return RedirectToAction("Shop", new { date });
    }

    [HttpPost("remove-from-basket")]
    public IActionResult RemoveFromBasket(int productId, DateOnly date)
    {
        logger.LogInformation("RemoveFromBasket action called with productId: {ProductId} and date: {Date}", productId, date);
        basketService.RemoveFromBasket(productId);
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }
}