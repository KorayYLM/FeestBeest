using System.Security.Claims;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using FeestBeest.Data.Dtos;
using FeestBeest.Data.Models;

namespace FeestBeest.Web.Controllers;

[Route("/shop")]
public class OrderController(ProductService productService, BasketService basketService, OrderService orderService, AccountService accountService) : Controller
{
    [HttpGet]
    public IActionResult Index(string? message = "")
    {
        ViewBag.Message = message;
        basketService.ClearBasket();
        return View();
    }

    [HttpPost]
    public IActionResult IndexPost(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now))
        {
            ViewBag.Message = "You can't order for a date in the past";
            return View("Index");
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<ProductType>? selectedTypes)
    {
        var productDtos = productService.GetProducts(date, selectedTypes);
        var products = productDtos.Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type, // Use the enum directly
            Img = dto.Img,
            IsInBasket = basketService.GetBasketProducts().Any(bp => bp.Id == dto.Id)
        }).ToList();

        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = selectedTypes ?? new List<ProductType>(),
                BasketCount = basketService.GetBasketItemCount()
            }
        };
        return View(model);
    }

    [HttpGet("contact")]
    public IActionResult Contact(DateOnly date)
    {
        var products = basketService.GetBasketProducts().Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type, // Use the enum directly
            Img = dto.Img,
            IsInBasket = true // Set IsInBasket to true for basket products
        }).ToList();

        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = new List<ProductType>(),
                BasketCount = basketService.GetBasketItemCount()
            },
        };
        return View(model);
    }

    [HttpPost("contact")]
    public IActionResult ContactPost(OrderViewModel model, bool skip)
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
                model.HouseNumber = account.HouseNumber;
                model.PhoneNumber = account.PhoneNumber;

                ModelState.Clear();
                TryValidateModel(model);
            }
        }

        var products = basketService.GetBasketProducts().Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type, // Use the enum directly
            Img = dto.Img,
            IsInBasket = true // Set IsInBasket to true for basket products
        }).ToList();

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = products,
        };

        return ModelState.IsValid ? RedirectToAction("Confirmation", model) : RedirectToAction("Contact");
    }

    [HttpGet("confirmation")]
    public IActionResult Confirmation(OrderViewModel model)
    {
        var products = basketService.GetBasketProducts().Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type, // Use the enum directly
            Img = dto.Img,
            IsInBasket = true // Set IsInBasket to true for basket products
        }).ToList();

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = products,
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;
        model.DiscountAmount = model.TotalPrice * (100 - orderService.DiscountCheckRules(parsedId, model.ToDto())) / 100;

        return View(model);
    }

    [HttpPost("confirmation")]
    public IActionResult ConfirmationPost(OrderViewModel model)
    {
        var products = basketService.GetBasketProducts().Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type, // Use the enum directly
            Img = dto.Img,
            IsInBasket = true // Set IsInBasket to true for basket products
        }).ToList();

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = products,
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;

        var (check, result) = orderService.CreateOrder(model.ToDto(), parsedId);
        if (check)
        {
            basketService.ClearBasket();
        }
        model.Check = check;
        model.Result = result;
        return RedirectToAction("Confirmation", model); 
    }

    [HttpPost]
    [Route("AddToBasket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            basketService.AddToBasket(product);
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }

    [HttpPost]
    [Route("RemoveFromBasket")]
    public IActionResult RemoveFromBasket(int productId, DateOnly date)
    {
        basketService.RemoveFromBasket(productId);
        return RedirectToAction("Shop", new { date, selectedTypes = new List<ProductType>() });
    }
}