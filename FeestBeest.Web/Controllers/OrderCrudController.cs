using FeestBeest.Web.Models;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FeestBeest.Data.Dto;

namespace FeestBeest.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("order-crud")]
    public class OrderCrudController : Controller
    {
        private readonly OrderService _orderService;

        public OrderCrudController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = GetUserId();
            var orders = _orderService.GetAllOrderByUserId(userId);

            var model = new OrdersOverviewViewModel
            {
                Orders = orders
            };

            return View(model);
        }

        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id)
        {
            var order = _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = MapOrderToViewModel(order);
            return View(model);
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }

        private static OrderViewModel MapOrderToViewModel(OrderDto order)
        {
            return new OrderViewModel
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
        }
    }
}