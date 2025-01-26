using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class CheckOrderProductsRule : ValidationAttribute
{
    private const int NoneMax = 3;
    private const int SilverMax = 4;

    public (bool, string) CheckProducts(Basket basket, User? user, ProductDto? newProduct = null)
    {
        var amountOfProducts = basket.Products.Count + 1;
        var hasVipProduct = HasVipProduct(basket, newProduct);

        if (user == null)
        {
            return ValidateNoneUser(amountOfProducts, hasVipProduct);
        }

        if (newProduct != null && newProduct.Type == ProductType.VIP && user.Rank != Rank.PLATINUM)
        {
            return (false, "Only VIP users can add VIP products to the basket.");
        }

        return ValidateUserRank(user, amountOfProducts, hasVipProduct);
    }

    private bool HasVipProduct(Basket basket, ProductDto? newProduct)
    {
        return basket.Products.Any(p => p.Type == ProductType.VIP) || (newProduct != null && newProduct.Type == ProductType.VIP);
    }

    private (bool, string) ValidateNoneUser(int amountOfProducts, bool hasVipProduct)
    {
        return amountOfProducts <= NoneMax && !hasVipProduct
            ? (true, string.Empty)
            : (false, $"You have too many products in your order, only {NoneMax} products are allowed and no VIP products.");
    }

    private (bool, string) ValidateUserRank(User user, int amountOfProducts, bool hasVipProduct)
    {
        return user.Rank switch
        {
            Rank.PLATINUM => (true, string.Empty),
            Rank.GOLD => !hasVipProduct ? (true, string.Empty) : (false, "Gold members cannot have VIP products."),
            Rank.SILVER => amountOfProducts <= SilverMax && !hasVipProduct
                ? (true, string.Empty)
                : (false, $"Silver members can only have up to {SilverMax} products and no VIP products."),
            Rank.NONE => amountOfProducts <= NoneMax && !hasVipProduct
                ? (true, string.Empty)
                : (false, $"You have too many products in your order, only {NoneMax} products are allowed and no VIP products."),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}