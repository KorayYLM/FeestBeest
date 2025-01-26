using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class ProductsNotTogetherRule
{
    private static readonly string[] Animals = { "leeuw", "ijsbeer" };
    private const string ErrorMessage = "Nom Nom Nom";

    public (bool, string) CheckProductsTogether(Basket basket, ProductDto product)
    {
        if (IsAnimal(product))
        {
            if (ContainsFarmProduct(basket))
            {
                return (false, ErrorMessage);
            }
        }
        else if (IsFarmProduct(product))
        {
            if (ContainsAnimal(basket))
            {
                return (false, ErrorMessage);
            }
        }
        return (true, string.Empty);
    }

    private bool IsAnimal(ProductDto product)
    {
        return Animals.Contains(product.Name.ToLower());
    }

    private bool IsFarmProduct(ProductDto product)
    {
        return product.Type == ProductType.FARM;
    }

    private bool ContainsAnimal(Basket basket)
    {
        return basket.Products.Any(p => Animals.Contains(p.Name.ToLower()));
    }

    private bool ContainsFarmProduct(Basket basket)
    {
        return basket.Products.Any(p => p.Type == ProductType.FARM);
    }
}