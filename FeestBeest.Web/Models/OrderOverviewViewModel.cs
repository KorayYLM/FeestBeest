using FeestBeest.Data.Dto;

namespace FeestBeest.Web.Models;    

public class OrdersOverviewViewModel
{
    public IEnumerable<OrderDto> Orders { get; set; }
}