using System.Security.Claims;
using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeestBeest.Web.Controllers;

[Route("/shop")]
public class OrderController : Controller
{
    private readonly ProductService _productService;
    private readonly BasketService _basketService;
    private readonly OrderService _orderService;
    private readonly AccountService _accountService;

    public OrderController(ProductService productService, BasketService basketService, OrderService orderService, AccountService accountService)
    {
        _productService = productService;
        _basketService = basketService;
        _orderService = orderService;
        _accountService = accountService;
    }

    [HttpGet("")]
    public IActionResult Index(string? message = "")
    {
        ViewBag.Message = message;
        _basketService.Clear();
        ViewData["step"] = "Choose date";

        var model = new OrderViewModel
        {
            Date = DateTime.Now
        };

        return View(model);
    }

    [HttpPost("index-post")]
    public IActionResult IndexPost(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now))
        {
            ViewData["step"] = "Choose date";
            return View("Index", new OrderViewModel { Date = DateTime.Now });
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }

    [HttpGet("shop")]
    public IActionResult Shop(DateOnly date, string? result, bool check = true)
    {
        ViewData["step"] = "Select Products";
        var products = _productService.GetProducts(date);
        var basketProducts = _basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.InBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        var model = new OrderViewModel
        {
            OrderFor = date,
            Check = check,
            Result = result,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                BasketCount = _basketService.GetBasketCount()
            }
        };
        return View(model);
    }

    [HttpGet("contact-info")]
    public IActionResult ContactInfo(DateOnly date, OrderViewModel? OVmodel = null)
    {
        ViewData["step"] = "Contact information";
        var model = OVmodel ?? new OrderViewModel();
        model.OrderFor = date;
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = _basketService.GetBasketProducts(),
            SelectedTypes = new List<ProductType>(),
            BasketCount = _basketService.GetBasketCount()
        };
        return View(model);
    }

    [HttpPost("contact-info-post")]
    public IActionResult ContactInfoPost(OrderViewModel model, bool skip)
    {
        ViewData["step"] = "Contact information";
        if (skip)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var parsedId = int.Parse(userId);
                var account = _accountService.GetUserById(parsedId);
                model.Name = account.Name;
                model.Email = account.Email;
                model.ZipCode = account.ZipCode;
                model.HouseNumber = account.HouseNumber;
                model.PhoneNumber = account.PhoneNumber;

                ModelState.Clear();
                TryValidateModel(model);
            }
        }

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = _basketService.GetBasketProducts(),
        };

        return ModelState.IsValid ? RedirectToAction("Confirm", model) : RedirectToAction("ContactInfo", new { date = model.OrderFor.ToString("yyyy-MM-dd"), OVmodel = model });
    }

    [HttpGet("confirm")]
    public IActionResult Confirm(OrderViewModel model)
    {
        ViewData["step"] = "Confirmation";
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = _basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;
        model.DiscountAmount = model.TotalPrice * _orderService.DiscountCheckRules(parsedId, model.ToDto()) / 100;
        return View(model);
    }

    [HttpPost("confirm-post")]
    public IActionResult ConfirmPost(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = _basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;

        _orderService.CreateOrder(model.ToDto(), parsedId);
        _basketService.Clear();

        return RedirectToAction("Index", new { message = "Order created successfully" });
    }

    [HttpPost("add-to-basket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = _productService.GetProductById(productId);
        if (product != null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parsedId = userId != null ? int.Parse(userId) : (int?)null;
            var (isValid, message) = _basketService.Add(product, parsedId);
            if (!isValid)
            {
                return RedirectToAction("Shop", new { date, result = message, check = isValid });
            }
            product.InBasket = true;
        }
        return RedirectToAction("Shop", new { date });
    }

    [HttpPost("remove-from-basket")]
    public IActionResult RemoveFromBasket(int productId, DateOnly date)
    {
        _basketService.Remove(productId);
        return RedirectToAction("Shop", new {date});
    }
}