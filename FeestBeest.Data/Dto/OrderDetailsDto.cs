
namespace FeestBeest.Data.Dto;  

public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } = null!;
}