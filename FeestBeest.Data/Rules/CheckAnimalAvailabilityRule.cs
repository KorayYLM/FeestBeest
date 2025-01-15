using FeestBeest.Data.Dtos;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class CheckAnimalAvailabilityRule
{
    private const string WeekdayOnlyMessage = "Dieren in pak werken alleen doordeweek";
    private const string TooColdMessage = "Veelste koud";
    private const string MeltingMessage = "Some People Are Worth Melting For. ~ Ola";

    // Check if the animal is available in the current season
    public (bool, string) CheckAnimalAvailability(Basket basket, ProductDto product)
    {
        var currentDate = DateTime.Now;
        var dayOfWeek = currentDate.DayOfWeek;
        var month = currentDate.Month;

        // Check the new product
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

        // Check the products already in the basket
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