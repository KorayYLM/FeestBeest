using FeestBeest.Data.Dto;

namespace FeestBeest.Data.Rules;

public class NameRule
{
    private static readonly Random random = new Random();
    private const string TargetName = "eend";
    private const int DiscountPercentage = 50;
    private const int Chance = 6;

    public int CheckForName(OrderDto orderDto)
    {
        return ContainsTargetName(orderDto) && IsLucky() ? DiscountPercentage : 0;
    }

    private bool ContainsTargetName(OrderDto orderDto)
    {
        return orderDto.OrderDetails.Any(p => p.Product.Name.Equals(TargetName, StringComparison.OrdinalIgnoreCase));
    }

    private bool IsLucky()
    {
        return random.Next(1, Chance + 1) == 1;
    }
}