
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public int Price { get; set; }
        public string Img { get; set; }
        public bool InBasket { get; set; }

    }
}