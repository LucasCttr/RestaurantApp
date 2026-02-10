using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<ProductIngredient> ProductIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });
        
        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductIngredients)
            .HasForeignKey(pi => pi.ProductId);

        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(i => i.ProductIngredients)
            .HasForeignKey(pi => pi.IngredientId);
    

    modelBuilder.Entity<Category>().HasData(
        new Category { CategoryId = 1, Name = "Plato" },   
        new Category { CategoryId = 2, Name = "Bebida" },
        new Category { CategoryId = 3, Name = "Postre" },
        new Category { CategoryId = 4, Name = "Entrada" },
        new Category { CategoryId = 5, Name = "Bebida" },
        new Category { CategoryId = 6, Name = "Postre" },
        new Category { CategoryId = 7, Name = "Entrada" },
        new Category { CategoryId = 8, Name = "Bebida" },
        new Category { CategoryId = 9, Name = "Postre" },
        new Category { CategoryId = 10, Name = "Entrada" });
  

    modelBuilder.Entity<Ingredient>().HasData(
        new Ingredient { IngredientId = 1, Name = "Lechuga" },
        new Ingredient { IngredientId = 2, Name = "Tomate" },
        new Ingredient { IngredientId = 3, Name = "Cebolla" },
        new Ingredient { IngredientId = 4, Name = "Carne" },
        new Ingredient { IngredientId = 5, Name = "Pollo" },
        new Ingredient { IngredientId = 6, Name = "Pescado" },
        new Ingredient { IngredientId = 7, Name = "Verdura" },
        new Ingredient { IngredientId = 8, Name = "Fruta" },
        new Ingredient { IngredientId = 9, Name = "Queso" },
        new Ingredient { IngredientId = 10, Name = "Huevo" });
    
    modelBuilder.Entity<Product>().HasData(
        new Product { ProductId = 1, Name = "Hamburguesa", Description = "Hamburguesa con queso", Price = 10, Stock = 100, CategoryId = 1 },
        new Product { ProductId = 2, Name = "Pizza", Description = "Pizza con queso", Price = 10, Stock = 100, CategoryId = 2 },
        new Product { ProductId = 3, Name = "Helado", Description = "Helado con chocolate", Price = 10, Stock = 100, CategoryId = 3 },
        new Product { ProductId = 4, Name = "Ensalada", Description = "Ensalada con lechuga", Price = 10, Stock = 100, CategoryId = 4 },
        new Product { ProductId = 5, Name = "Coca-Cola", Description = "Coca-Cola 350ml", Price = 10, Stock = 100, CategoryId = 5 },
        new Product { ProductId = 6, Name = "Pepsi", Description = "Pepsi 350ml", Price = 10, Stock = 100, CategoryId = 6 },
        new Product { ProductId = 7, Name = "Fanta", Description = "Fanta 350ml", Price = 10, Stock = 100, CategoryId = 7 },
        new Product { ProductId = 8, Name = "Sprite", Description = "Sprite 350ml", Price = 10, Stock = 100, CategoryId = 8 },
        new Product { ProductId = 9, Name = "Agua", Description = "Agua 350ml", Price = 10, Stock = 100, CategoryId = 9 },
        new Product { ProductId = 10, Name = "Vino", Description = "Vino 350ml", Price = 10, Stock = 100, CategoryId = 10 });
    

    modelBuilder.Entity<ProductIngredient>().HasData(
    // Hamburguesa (1) - Carne, Queso, Lechuga, Tomate, Cebolla
    new ProductIngredient { ProductId = 1, IngredientId = 1 }, // Lechuga
    new ProductIngredient { ProductId = 1, IngredientId = 2 }, // Tomate
    new ProductIngredient { ProductId = 1, IngredientId = 3 }, // Cebolla
    new ProductIngredient { ProductId = 1, IngredientId = 4 }, // Carne
    new ProductIngredient { ProductId = 1, IngredientId = 9 }, // Queso

    // Pizza (2) - Queso, Tomate, Carne
    new ProductIngredient { ProductId = 2, IngredientId = 2 }, // Tomate
    new ProductIngredient { ProductId = 2, IngredientId = 4 }, // Carne
    new ProductIngredient { ProductId = 2, IngredientId = 9 }, // Queso

    // Helado (3) - Fruta (helado de chocolate con frutas)
    new ProductIngredient { ProductId = 3, IngredientId = 8 }, // Fruta

    // Ensalada (4) - Lechuga, Tomate, Cebolla, Verdura, Huevo
    new ProductIngredient { ProductId = 4, IngredientId = 1 }, // Lechuga
    new ProductIngredient { ProductId = 4, IngredientId = 2 }, // Tomate
    new ProductIngredient { ProductId = 4, IngredientId = 3 }, // Cebolla
    new ProductIngredient { ProductId = 4, IngredientId = 7 }, // Verdura
    new ProductIngredient { ProductId = 4, IngredientId = 10 }); // Huevo
    }
}

