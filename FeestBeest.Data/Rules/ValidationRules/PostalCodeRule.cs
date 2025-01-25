using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FeestBeest.Data.Rules.ValidationRules;

public class PostalCodeRule : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        String postalCode = value.ToString().Replace(" ", "");
        return Regex.IsMatch(postalCode, @"^\d{4}[A-Za-z]{2}$"); // 5432DF
    }
}