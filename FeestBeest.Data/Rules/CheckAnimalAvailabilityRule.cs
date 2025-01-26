using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class CheckAnimalAvailabilityRule
{
    private const string WeekDayMess = "This product is only available on weekdays"; 
    private const string ColdMess = "Its to cold right now for this product"; 
    private const string MeltMess = "Metlinggg";

    public (bool, string) CheckAnimalAvailability(Basket basket, ProductDto product)
    {
        var currentDate = DateTime.Now;
        var dayOfWeek = currentDate.DayOfWeek;
        var month = currentDate.Month;

        if (!IsProductAvailable(product, dayOfWeek, month))
        {
            return (false, GetAvailabilityMessage(product, dayOfWeek, month));
        }

        foreach (var productInBasket in basket.Products)
        {
            if (!IsProductAvailable(productInBasket, dayOfWeek, month))
            {
                return (false, GetAvailabilityMessage(productInBasket, dayOfWeek, month));
            }
        }

        return (true, string.Empty);
    }

    private bool IsProductAvailable(ProductDto product, DayOfWeek dayOfWeek, int month)
    {
        if (product.Name.Equals("Pinguïn") && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
        {
            return false;
        }

        if (product.Type.ToString() == ProductType.DESERT.ToString() && (month >= 10 || month <= 2))
        {
            return false;
        }

        if (product.Type.ToString() == ProductType.SNOW.ToString() && (month >= 6 && month <= 8))
        {
            return false;
        }

        return true;
    }

    private string GetAvailabilityMessage(ProductDto product, DayOfWeek dayOfWeek, int month)
    {
        if (product.Name.Equals("Pinguïn") && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
        {
            return WeekDayMess;
        }

        if (product.Type.ToString() == ProductType.DESERT.ToString() && (month >= 10 || month <= 2))
        {
            return ColdMess;
        }

        if (product.Type.ToString() == ProductType.SNOW.ToString() && (month >= 6 && month <= 8))
        {
            return MeltMess;
        }

        return string.Empty;
    }
}