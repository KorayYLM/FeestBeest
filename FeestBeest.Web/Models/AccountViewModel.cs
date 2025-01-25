using FeestBeest.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace FeestBeest.Web.Models
{
    public class AccountViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Rank Rank { get; set; }

        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
    }
}