
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class IngredientController : Controller
    {
        private Repository<Ingredient> ingredients;

        public IngredientController(AppDbContext context)
        {
            this.ingredients = new Repository<Ingredient>(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var options = new QueryOptions<Ingredient>();
            options.Includes = "ProductIngredients,ProductIngredients.Product";
            var ingredient = await ingredients.GetByIdAsync(id, options);
            if (ingredient == null)
                return NotFound();
            return View(ingredient);
        } 

        // Ingredient/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientId, Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.AddAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
            
        }
    }
}