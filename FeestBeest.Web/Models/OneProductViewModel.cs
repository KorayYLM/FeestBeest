using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models
{
    public class OneProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s\p{P}]+$", ErrorMessage = "Name can only contain letters and punctuation.")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000.")]
        public int Price { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Required]
        public string Img { get; set; } = null!;

        public bool Check { get; set; }
        public string? Result { get; set; }

        public List<OrderDto>? Orders { get; set; } = new List<OrderDto>();
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

        public List<string> AvailableImages { get; set; } = new List<string>();
        public List<ProductType> AvailableTypes { get; set; } = new List<ProductType>();

        public ProductDto ToDto()
        {
            return new ProductDto()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Type = Type,
                Img = Img
            };
        }
    }
}