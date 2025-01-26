using FeestBeest.Data.Dto;

public class CheckDayOfWeekRule
{
    private const int DiscountPercentage = 15;

    public int IsDayOfWeek(OrderDto orderDto)
    {
        return IsDiscountDay(orderDto.OrderFor.DayOfWeek) ? DiscountPercentage : 0;
    }

    private bool IsDiscountDay(DayOfWeek dayOfWeek)
    {
        return dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday;
    }
}