
using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models;    


public class ProductViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ProductType Type { get; set; }

    [Range(0, 10000)]
    public int Price { get; set; }

    [Required] public string Img { get; set; } = null!;

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