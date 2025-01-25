using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class ProductsNotTogetherRule
{
    public (bool, string) CheckProductsTogether(Basket basket, ProductDto product)
    {
        var dangerousAnimals = new[] { "leeuw", "ijsbeer" }; 

        if (dangerousAnimals.Contains(product.Name.ToLower()))
        {
            foreach (var product2 in basket.Products)
            {
                if (product2.Type.ToString() == ProductType.FARM.ToString())
                {
                    return (false, "Nom Nom Nom");
                }
            }
        }
        else if (product.Type.ToString() == ProductType.FARM.ToString())
        {
            foreach (var product2 in basket.Products)
            {
                if (dangerousAnimals.Contains(product2.Name.ToLower()))
                {
                    return (false, "Nom Nom Nom");
                }
            }
        }
        return (true, string.Empty);
    }
}