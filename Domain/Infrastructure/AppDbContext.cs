using Microsoft.EntityFrameworkCore;
using RecipeBook_API.Domain.Entities;

namespace RecipeBook_API.Domain.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<InstructionStep> Steps => Set<InstructionStep>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Recipe>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(120);
                e.OwnsOne(x => x.Nutrition);
                e.HasMany(x => x.Ingredients).WithOne().HasForeignKey(i => i.RecipeId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Steps).WithOne().HasForeignKey(s => s.RecipeId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Tags).WithMany(t => t.Recipes);
                e.HasIndex(x => x.Title);
            });

            b.Entity<Tag>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.HasIndex(x => x.Name).IsUnique();
            });

            b.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Email).IsRequired().HasMaxLength(200);
                e.HasIndex(x => x.Email).IsUnique();
            });

            b.Entity<Ingredient>().HasKey(x => x.Id);
            b.Entity<InstructionStep>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.RecipeId, x.Order }).IsUnique();
            });
        }
    }
}