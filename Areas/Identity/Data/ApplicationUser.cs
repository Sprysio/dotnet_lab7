using Microsoft.AspNetCore.Identity;

namespace lab7.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public long CustomerId { get; set; }
    }
}