using Microsoft.AspNetCore.Identity;

namespace AgriEnergyP2.Models
{
    public class ApplicationUser: IdentityUser
    {
        public Farmer FarmerProfile { get; set; } // One-to-one relationship
    }
}
