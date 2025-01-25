using FeestBeest.Data.Dto;

public class CheckDayOfWeekRule
{
    //monday, tuesday 15% discount  
    public int IsDayOfWeek(OrderDto orderDto)
    {
        return orderDto.OrderFor.DayOfWeek == DayOfWeek.Monday || orderDto.OrderFor.DayOfWeek == DayOfWeek.Tuesday ? 15 : 0;
    }
}