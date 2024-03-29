using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        //you can add more properties into the Identity user
        public int Risk { get; set; }
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}