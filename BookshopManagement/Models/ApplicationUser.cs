using Microsoft.AspNetCore.Identity;

namespace BookshopManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add extra properties if you want, e.g. FullName
        public required string FullName { get; set; }
    }
}
