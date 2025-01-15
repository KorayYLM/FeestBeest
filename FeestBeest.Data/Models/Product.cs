using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ProductType Type { get; set; }
    public int Price { get; set; }
    public string Img { get; set; }
    public bool IsInBasket { get; set; }

}