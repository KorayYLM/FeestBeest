
namespace FeestBeest.Data.Dto;  

public class OrderDetailsDto
{
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } = null!;
}