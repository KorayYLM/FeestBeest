using FeestBeest.Data;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Dtos;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Identity;

namespace  FeestBeest.Data.Services;    

public class OrderService
{
    private readonly FeestBeestContext _context;
    private readonly UserManager<FeestBeest.Data.Models.User> _userManager;
    private readonly ProductService _productService;

    public OrderService(FeestBeestContext context, UserManager<FeestBeest.Data.Models.User> userManager, ProductService productService)
    {
        _context = context;
        _userManager = userManager;
        _productService = productService;
    }

    public (bool, string) CreateOrder(OrderDto orderDto, int? userId = null)
    {
        if (orderDto != null)
        {
            var order = new Order()
            {
                Name = orderDto.Name,
                Email = orderDto.Email,
                ZipCode = orderDto.ZipCode,
                HouseNumber = orderDto.HouseNumber,
                PhoneNumber = orderDto.PhoneNumber,
                OrderFor = orderDto.OrderFor,
                UserId = userId,
                TotalPrice = CalculateTotalPrice(orderDto, userId),
                OrderDetails = orderDto.OrderDetails.Select(x => new OrderDetail()
                {
                    ProductId = x.ProductId,
                    Product = _context.Products.FirstOrDefault(p => p.Id == x.ProductId)
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            return (true, "Order created successfully");
        }
        return (false, "Order creation failed");
    }

    private int CalculateTotalPrice(OrderDto orderDto, int? userId)
    {
        var discount = DiscountCheckRules(userId, orderDto);
        return orderDto.TotalPrice * (100 - discount) / 100;
    }

    public int DiscountCheckRules(int? userId, OrderDto orderDto)
    {
        var user = userId != null ? _userManager.FindByIdAsync(userId.ToString()).Result : null;

        var totalDiscount = new HasRankRule().UserHasRank(user)
                            + new CheckDayOfWeekRule().IsDayOfWeek(orderDto)
                            + new SameTypeRule().CheckSameType(orderDto)
                            + new NameContainsRule().ApplyNameContainsDiscount(orderDto)
                            + new CheckNameRule().CheckForName(orderDto);

        return totalDiscount <= 60 ? totalDiscount : 60;
    }

    private IQueryable<OrderDto> SelectAllOrders()
    {
        return _context.Orders
            .Select(x => new OrderDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ZipCode = x.ZipCode,
                HouseNumber = x.HouseNumber,
                PhoneNumber = x.PhoneNumber,
                OrderFor = x.OrderFor,
                TotalPrice = x.TotalPrice,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId,
                    Product = new ProductDto()
                    {
                        Id = y.Product.Id,
                        Name = y.Product.Name,
                        Type = y.Product.Type,
                        Price = y.Product.Price,
                        Img = y.Product.Img
                    }
                }).ToList()
            });
    }

    public List<OrderDto> GetAllOrders()
    {
        return SelectAllOrders().ToList();
    }

    public OrderDto? GetOrder(int id)
    {
        return SelectAllOrders().FirstOrDefault(x => x.Id == id);
    }

    public void DeleteOrder(int id)
    {
        var order = _context.Orders.FirstOrDefault(x => x.Id == id);

        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }

    public List<OrderDto> GetAllOrderByUserId(int id)
    {
        return _context.Orders
            .Where(x => x.UserId == id)
            .Select(x => new OrderDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ZipCode = x.ZipCode,
                HouseNumber = x.HouseNumber,
                PhoneNumber = x.PhoneNumber,
                OrderFor = x.OrderFor,
                TotalPrice = x.TotalPrice,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId
                }).ToList()
            })
            .ToList();
    }

    public List<OrderDto> GetAllOrdersByProductId(int id)
    {
        return SelectAllOrders()
            .Where(x => x.OrderDetails.Any(y => y.ProductId == id))
            .ToList();
    }
}