using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;

namespace FeestBeest.Web.Models;    

public class OrderViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.PostalCode)]
    public string ZipCode { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string HouseNumber { get; set; } = null!;

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly OrderFor { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public ProductsOverViewModel ProductsOverViewModel { get; set; } = new();
    public List<Product> Products { get; set; } = new(); 

    public int TotalPrice { get; set; }
    public int DiscountAmount { get; set; }

    public string? Result { get; set; }
    public bool Check { get; set; }

    public OrderDto ToDto()
    {
        return new OrderDto()
        {
            Id = this.Id,
            Name = this.Name,
            Email = this.Email,
            ZipCode = this.ZipCode,
            HouseNumber = this.HouseNumber,
            PhoneNumber = this.PhoneNumber,
            OrderFor = this.OrderFor,
            TotalPrice = this.TotalPrice,
            OrderDetails = this.ProductsOverViewModel.Products
                .Where(p => p.InBasket)
                .Select(p => new OrderDetailsDto()
                {
                    ProductId = p.Id,
                    Product = p
                })
                .ToList()
        };
    }
}