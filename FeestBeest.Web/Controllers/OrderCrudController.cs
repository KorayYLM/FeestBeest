using FeestBeest.Web.Models;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FeestBeest.Data.Dto;

namespace FeestBeest.Web.Controllers
{
    [Authorize]
    [Route("order-crud")]
    public class OrderCrudController : Controller
    {
        private readonly OrderService orderService;
        private readonly ILogger<OrderCrudController> logger;

        public OrderCrudController(OrderService orderService, ILogger<OrderCrudController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = orderService.GetAllOrderByUserId(int.Parse(userId));

            var model = new OrdersOverviewViewModel
            {
                Orders = orders
            };

            return View(model);
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var orderDto = orderService.GetOrder(id);
            if (orderDto == null)
            {
                return NotFound();
            }

            var orderViewModel = new OrderViewModel
            {
                Id = orderDto.Id,
                Name = orderDto.Name,
                Email = orderDto.Email,
                ZipCode = orderDto.ZipCode,
                HouseNumber = orderDto.HouseNumber,
                PhoneNumber = orderDto.PhoneNumber,
                OrderFor = orderDto.OrderFor,
                TotalPrice = orderDto.TotalPrice,
                ProductsOverViewModel = new ProductsOverViewModel
                {
                    Products = orderDto.OrderDetails.Select(od => new ProductDto
                    {
                        Id = od.Product.Id,
                        Name = od.Product.Name,
                        Type = od.Product.Type,
                        Price = od.Product.Price,
                        Img = od.Product.Img
                    }).ToList()
                }
            };

            return View(orderViewModel);
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            orderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}