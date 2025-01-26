using FeestBeest.Data.Dto;

public class ProductNameContainsUQ
{
    private const int DiscountPerUniqueLetter = 2;

    public int ApplyNameContainsDiscount(OrderDto orderDto)
    {
        var uniqueLetters = GetUniqueLetters(orderDto);
        return uniqueLetters.Count * DiscountPerUniqueLetter;
    }

    private HashSet<char> GetUniqueLetters(OrderDto orderDto)
    {
        var uniqueLetters = new HashSet<char>();

        foreach (var product in orderDto.OrderDetails.Select(p => p.Product))
        {
            foreach (var letter in product.Name.ToLower())
            {
                uniqueLetters.Add(letter);
            }
        }

        return uniqueLetters;
    }
}