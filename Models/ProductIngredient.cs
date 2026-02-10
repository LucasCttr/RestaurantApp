namespace RestaurantApp.Models;

public class ProductIngredient {
    public int IngredientId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public Ingredient Ingredient { get; set; }
}