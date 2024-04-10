using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Models;

namespace RecipeBook.Areas.Identity.Data;

public class RecipeBookContext : IdentityDbContext<RecipeBookUser>
{
    public RecipeBookContext(DbContextOptions<RecipeBookContext> options)
        : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Direction> Directions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>().ToTable("Recipe");
        modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
        modelBuilder.Entity<Direction>().ToTable("Direction");
    }
}
