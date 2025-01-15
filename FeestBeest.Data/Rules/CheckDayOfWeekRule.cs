﻿using FeestBeest.Data.Dto;

public class CheckDayOfWeekRule
{
    //15% discount on Monday and Tuesday
    public int IsDayOfWeek(OrderDto orderDto)
    {
        return orderDto.OrderFor.DayOfWeek == DayOfWeek.Monday || orderDto.OrderFor.DayOfWeek == DayOfWeek.Tuesday ? 15 : 0;
    }
}