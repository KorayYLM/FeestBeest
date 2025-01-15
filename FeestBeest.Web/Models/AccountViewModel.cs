using FeestBeest.Data.Models;

namespace FeestBeest.Web.Models
{
    public class AccountViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Rank Rank { get; set; }
        public string HouseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
    }
}