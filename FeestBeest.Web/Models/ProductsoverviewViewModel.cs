using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models;    


public class ProductsOverViewModel
{
    public int BasketCount { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
    public List<ProductType> SelectedTypes { get; set; } = new();
}