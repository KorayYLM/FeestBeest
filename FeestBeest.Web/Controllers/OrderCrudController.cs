using FeestBeest.Web.Models;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FeestBeest.Data.Dto;

namespace FeestBeest.Web.Controllers
{
    [Authorize (Roles = "Customer")]   
    [Route("order-crud")]
    public class OrderCrudController : Controller
    {
        private readonly OrderService orderService;

        public OrderCrudController(OrderService orderService)
        {
            this.orderService = orderService;
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

        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderViewModel
            {
                Id = order.Id,
                Name = order.Name,
                Email = order.Email,
                ZipCode = order.ZipCode,
                HouseNumber = order.HouseNumber,
                PhoneNumber = order.PhoneNumber,
                OrderFor = order.OrderFor,
                TotalPrice = order.TotalPrice,
                ProductsOverViewModel = new ProductsOverViewModel
                {
                    Products = order.OrderDetails.Select(od => od.Product).ToList()
                }
            };

            return View(model);
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            orderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}