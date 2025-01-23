using FeestBeest.Data.Dto;

namespace FeestBeest.Data.Models;

public class OrderDetail
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}