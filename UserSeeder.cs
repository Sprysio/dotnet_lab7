using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using lab7.Areas.Identity.Data;
using lab7.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class UserSeeder
{
    public static async Task CopyUsers(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ChinookDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByEmailAsync(context.Customers.OrderBy(x => x.CustomerId).First().Email) == null)
            {
                foreach (var item in context.Customers)
                {
                    var user = new ApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        NormalizedEmail = item.Email,
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        CustomerId = item.CustomerId
                    };
                    await userManager.CreateAsync(user, "P@ssw0rd");
                }
            }
        }
    }
}