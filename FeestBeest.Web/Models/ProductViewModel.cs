
using FeestBeest.Data.Dtos;
using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models;    


public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ProductType Type { get; set; } 
    public int Price { get; set; }
    public string Img { get; set; } = null!;

    public static ProductViewModel FromDto(ProductDto dto)
    {
        return new ProductViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Type = dto.Type,
            Price = dto.Price,
            Img = dto.Img
        };
    }

    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Id = this.Id,
            Name = this.Name,
            Type = this.Type,
            Price = this.Price,
            Img = this.Img
        };
    }
}