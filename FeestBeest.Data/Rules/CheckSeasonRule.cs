using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class CheckSeasonRule
{
    // Check if the animal is available in the current season
    public (bool, string) CheckAnimalAvailability(OrderDto orderDto)
    {
        var currentDate = DateTime.Now;
        var dayOfWeek = currentDate.DayOfWeek;
        var month = currentDate.Month;

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            var productType = orderDetail.Product.Type;

            if (orderDetail.Product.Name == "Pinguïn" && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
            {
                return (false, "Dieren in pak werken alleen doordeweek");
            }

            if (productType.ToString() == ProductType.DESERT.ToString() && (month >= 10 || month <= 2))
            {
                return (false, "Veelste koud");
            }

            if (productType.ToString() == ProductType.SNOW.ToString() && (month >= 6 && month <= 8))
            {
                return (false, "Error");
            }
        }

        return (true, string.Empty);
    }
}