using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FeestBeest.Data.Rules.ValidationRules;

public class PhoneNumberRule : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        string phoneNumber = value.ToString();
        return Regex.IsMatch(phoneNumber, @"^\d{10,15}$"); // Matches phone numbers with 10 to 15 digits  
    }
}