using FeestBeest.Data.Models;
using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Rules.ValidationRules;

namespace FeestBeest.Web.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Rank is required.")]
        public Rank Rank { get; set; }

        [Required(ErrorMessage = "House number is required.")]
        [StringLength(10, ErrorMessage = "House number cannot be longer than 10 characters.")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [PhoneNumberRule(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid zip code format.")]
        public string ZipCode { get; set; }
    }
}