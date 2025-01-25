using FeestBeest.Data;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeestBeest.Data.Services;

public class ProductService
{
    private readonly FeestBeestContext _context;

    public ProductService(FeestBeestContext context)
    {
        _context = context;
    }

    public ProductDto GetProductById(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return null;
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Type = product.Type, 
            Price = product.Price,
            Img = product.Img
        };
    }

    public List<ProductDto> GetProducts(DateOnly? date = null, List<ProductType>? selectedTypes = null)
    {
        var products = _context.Products
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type, 
                Price = product.Price,
                Img = product.Img
            }).ToList();

        if (date != null)
        {
            var ordersOnDate = _context.Orders
                .Where(order => order.OrderFor == date)
                .Include(order => order.OrderDetails)
                .SelectMany(order => order.OrderDetails)
                .Select(orderDetail => orderDetail.ProductId)
                .ToList();

            products = products.Where(product => !ordersOnDate.Contains(product.Id)).ToList();
        }

        if (selectedTypes != null && selectedTypes.Any())
        {
            products = products.Where(p => selectedTypes.Contains(p.Type)).ToList();
        }

        return products;
    }

    public (bool, string) CreateProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Type = productDto.Type, 
            Price = productDto.Price,
            Img = productDto.Img
        };
        _context.Products.Add(product);
        _context.SaveChanges();
        return (true, "Product created");
    }

    public (bool, string) UpdateProduct(int id, ProductDto productDto)
    {
        var product = _context.Products.Find(id);
        if (product.Type != productDto.Type)
        {
            product.Img = productDto.Img;
        }
        product.Name = productDto.Name;
        product.Type = productDto.Type; 
        product.Price = productDto.Price;

        _context.SaveChanges();
        return (true, "Product updated");
    }

    public async Task DeleteBeestjeAsync(int id)
    {
        var beestje = await _context.Products.FindAsync(id);
        if (beestje == null)
        {
            return;
        }

        _context.Products.Remove(beestje);
        await _context.SaveChangesAsync();
    }

    public List<string> GetAvailableImages()
    {
        return _context.Products
            .Select(p => p.Img)
            .Distinct()
            .ToList();
    }
    
}