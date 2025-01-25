using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class CheckAnimalAvailabilityRule
{
    private const string WeekdayOnlyMessage = "This product is only available on weekdays"; 
    private const string TooColdMessage = "Its to cold right now for this product"; 
    private const string MeltingMessage = "Metlinggg";

    public (bool, string) CheckAnimalAvailability(Basket basket, ProductDto product)
    {
        var currentDate = DateTime.Now;
        var dayOfWeek = currentDate.DayOfWeek;
        var month = currentDate.Month;

        if (product.Name.Equals("Pinguïn") && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
        {
            return (false, WeekdayOnlyMessage);
        }

        if (product.Type.ToString() == ProductType.DESERT.ToString() && (month >= 10 || month <= 2))
        {
            return (false, TooColdMessage);
        }

        if (product.Type.ToString() == ProductType.SNOW.ToString() && (month >= 6 && month <= 8))
        {
            return (false, MeltingMessage);
        }

        foreach (var basketProduct in basket.Products)
        {
            if (basketProduct.Name.Equals("Pinguïn") && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
            {
                return (false, WeekdayOnlyMessage);
            }

            if (basketProduct.Type.ToString() == ProductType.DESERT.ToString() && (month >= 10 || month <= 2))
            {
                return (false, TooColdMessage);
            }

            if (basketProduct.Type.ToString() == ProductType.SNOW.ToString() && (month >= 6 && month <= 8))
            {
                return (false, MeltingMessage);
            }
        }

        return (true, string.Empty);
    }
}