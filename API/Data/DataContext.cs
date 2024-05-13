using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Entities.RecipeModuleEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<CheckLaterLink> CheckLaterLinks { get; set; }
        public DbSet<CheckLaterLinkCategory> CheckLaterLinkCategories { get; set; }
        public DbSet<RecipeDescriptionStep> RecipeDescriptionSteps { get; set; } // Add DbSet for RecipeDescriptionStep

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<CheckLaterLink>()
                .HasOne<CheckLaterLinkCategory>()
                .WithMany(clc => clc.CheckLaterLinks)
                .HasForeignKey(cll => cll.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Categories)
                .WithOne()
                .HasForeignKey(clc => clc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Links)
                .WithOne()
                .HasForeignKey(clc => clc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CheckLaterLinkCategory>()
                .HasMany(clc => clc.Subcategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(clc => clc.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Recipe and RecipeDescriptionStep
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.RecipeDescriptionSteps)
                .WithOne()
                .HasForeignKey(rds => rds.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
