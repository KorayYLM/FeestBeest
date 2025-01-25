using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;


namespace FeestBeest.Data.Rules;

public class CheckOrderProductsRule : ValidationAttribute
{
    private readonly int NONE_MAX = 3;
    private readonly int SILVER_MAX = 4;

    // Check if the order is valid
    public (bool, string) CheckProducts(Basket basket, User? user, ProductDto? newProduct = null)
    {
        var amountOfProducts = basket.Products.Count + 1;
        var hasVipProduct = basket.Products.Any(p => p.Type == ProductType.VIP) || (newProduct != null && newProduct.Type == ProductType.VIP);

        if (user == null)
        {
            return amountOfProducts <= NONE_MAX && !hasVipProduct ? (true, string.Empty) : (false, "You have too many products in your order, only " + NONE_MAX + " products are allowed and no VIP products.");
        }

        if (newProduct != null && newProduct.Type == ProductType.VIP && user.Rank != Rank.PLATINUM)
        {
            return (false, "Only VIP users can add VIP products to the basket.");
        }

        return user.Rank switch
        {
            Rank.PLATINUM => (true, string.Empty),
            Rank.GOLD => !hasVipProduct ? (true, string.Empty) : (false, "Gold members cannot have VIP products."),
            Rank.SILVER => amountOfProducts <= SILVER_MAX && !hasVipProduct ? (true, string.Empty) : (false, "Silver members can only have up to " + SILVER_MAX + " products and no VIP products."),
            Rank.NONE => amountOfProducts <= NONE_MAX && !hasVipProduct ? (true, string.Empty) : (false, "You have too many products in your order, only " + NONE_MAX + " products are allowed and no VIP products."),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}