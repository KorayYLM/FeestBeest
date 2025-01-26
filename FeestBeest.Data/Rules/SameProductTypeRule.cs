using FeestBeest.Data.Dto;

namespace FeestBeest.Data.Rules;

public class SameProductTypeRule
{
    private const int SameAmount = 3;
    private const int DiscountPercentage = 10;

    public int CheckSameType(OrderDto orderDto)
    {
        return HasSameTypeProducts(orderDto) ? DiscountPercentage : 0;
    }

    private bool HasSameTypeProducts(OrderDto orderDto)
    {
        return orderDto.OrderDetails
            .GroupBy(p => p.Product.Type)
            .Any(g => g.Count() >= SameAmount);
    }
}