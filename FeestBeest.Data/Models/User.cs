using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeestBeest.Data.Models
{
    public class User : IdentityUser<int>
    {
        public Rank Rank { get; set; }
        public string HouseNumber { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }
}