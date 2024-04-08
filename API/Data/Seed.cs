using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            var admin = new AppUser
            {
                UserName = "admin",
                MessageServiceRecipientId = "24830443579936750" 
            };

            await userManager.CreateAsync(admin, "123456");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });
        }

        // public static async Task AddCategory(DataContext context, UserManager<AppUser> userManager)
        // {
        //     var user = await userManager.FindByNameAsync("admin");

        //     if(user == null)
        //     {
        //         return;
        //     }
        //     var categories = new List<CheckLaterLinkCategory>
        //     {
        //         new CheckLaterLinkCategory{Name = "Bez kategorii", UserId = user.Id},
        //         new CheckLaterLinkCategory{Name = "Obejrzane", UserId = user.Id}
        //     };

        //     context.Entry(user).Collection(u => u.Categories).CurrentValue = categories;

        //     if(await context.CheckLaterLinkCategories.AnyAsync(t => t.Name == categories.First().Name))
        //     {
        //         return;
        //     }

        //     await context.CheckLaterLinkCategories.AddRangeAsync(categories);
            

        //     await context.SaveChangesAsync();

        // }
    }
}