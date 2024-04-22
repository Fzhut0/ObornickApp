using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class Seed
    {

        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"}
            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            

            var admin = new AppUser
            {  
                UserName = "admin",
                MessageServiceRecipientId = "24830443579936750",
            };            
            
            await userManager.CreateAsync(admin, password);
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });                   
        }
    }
}