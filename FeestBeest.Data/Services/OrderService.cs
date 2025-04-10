﻿using FeestBeest.Data;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using Microsoft.AspNetCore.Identity;

namespace FeestBeest.Data.Services
{
    public class OrderService
    {
        private readonly FeestBeestContext _context;
        private readonly UserManager<User> _userManager;

        public OrderService(FeestBeestContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void CreateOrder(OrderDto orderDto, int? userId = null)
        {
            if (orderDto == null) return;

            var order = MapOrderDtoToOrder(orderDto, userId);
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        private Order MapOrderDtoToOrder(OrderDto orderDto, int? userId)
        {
            return new Order
            {
                Name = orderDto.Name,
                Email = orderDto.Email,
                ZipCode = orderDto.ZipCode,
                HouseNumber = orderDto.HouseNumber,
                PhoneNumber = orderDto.PhoneNumber,
                OrderFor = orderDto.OrderFor,
                UserId = userId,
                TotalPrice = CalculateTotalPrice(orderDto, userId),
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    Product = _context.Products.FirstOrDefault(p => p.Id == od.ProductId)
                }).ToList()
            };
        }

        private int CalculateTotalPrice(OrderDto orderDto, int? userId)
        {
            var discount = DiscountCheckRules(userId, orderDto);
            return orderDto.TotalPrice * (100 - discount) / 100;
        }

        public int DiscountCheckRules(int? userId, OrderDto orderDto)
        {
            var user = userId.HasValue ? _userManager.FindByIdAsync(userId.Value.ToString()).Result : null;

            var totalDiscount = new RankRule().UserHasRank(user)
                                + new CheckDayOfWeekRule().IsDayOfWeek(orderDto)
                                + new SameProductTypeRule().CheckSameType(orderDto)
                                + new ProductNameContainsUQ().ApplyNameContainsDiscount(orderDto)
                                + new NameRule().CheckForName(orderDto);

            return totalDiscount <= 60 ? totalDiscount : 60;
        }

        private IQueryable<OrderDto> SelectAllOrders()
        {
            return _context.Orders
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ZipCode = x.ZipCode,
                    HouseNumber = x.HouseNumber,
                    PhoneNumber = x.PhoneNumber,
                    OrderFor = x.OrderFor,
                    TotalPrice = x.TotalPrice,
                    OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto
                    {
                        ProductId = y.ProductId,
                        Product = new ProductDto
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

        public OrderDto? GetOrder(int id)
        {
            return SelectAllOrders().FirstOrDefault(x => x.Id == id);
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return;

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public List<OrderDto> GetAllOrderByUserId(int id)
        {
            return _context.Orders
                .Where(x => x.UserId == id)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ZipCode = x.ZipCode,
                    HouseNumber = x.HouseNumber,
                    PhoneNumber = x.PhoneNumber,
                    OrderFor = x.OrderFor,
                    TotalPrice = x.TotalPrice,
                    OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto
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
}