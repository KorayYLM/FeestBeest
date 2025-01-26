using FeestBeest.Data.Models;

namespace FeestBeest.Data.Rules;

public class RankRule
{
    private const int DiscountPercentage = 10;

    public int UserHasRank(User? user)
    {
        return user != null && user.Rank != Rank.NONE ? DiscountPercentage : 0;
    }
}