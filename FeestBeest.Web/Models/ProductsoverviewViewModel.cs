using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models;    


public class ProductsOverViewModel
{
    public int BasketCount { get; set; }
    public List<Product> Products { get; set; } = new();
    public List<ProductType> SelectedTypes { get; set; } = new();
}