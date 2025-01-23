using FeestBeest.Data.Dto;

namespace FeestBeest.Data.Models
{
    public class Basket
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}