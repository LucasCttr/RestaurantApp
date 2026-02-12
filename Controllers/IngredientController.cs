
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
        // Lista todos los ingredientes.
        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAllAsync());
        }

        // Muestra los detalles de un ingrediente, incluyendo productos relacionados.
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
        // Muestra el formulario para crear un nuevo ingrediente.
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Procesa la creación de un nuevo ingrediente.
        public async Task<IActionResult> Create([Bind("IngredientId, Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.AddAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);

        }

        // Ingredient/Delete/{id}
        [HttpGet]
        // Muestra la confirmación para eliminar un ingrediente.
        public async Task<IActionResult> Delete(int id)
        {
            return View(await ingredients.GetByIdAsync(id, new QueryOptions<Ingredient>
            {
                Includes = "ProductIngredients,ProductIngredients.Product"
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Elimina el ingrediente confirmado.
        public async Task<IActionResult> Delete(Ingredient ingredient)
        {
            await ingredients.DeleteAsync(ingredient.IngredientId);
            return RedirectToAction(nameof(Index));
        }

        // Ingredient/Edit/{id}
        [HttpGet]
        // Muestra el formulario para editar un ingrediente.
        public async Task<IActionResult> Edit(int id)
        {
            return View(await ingredients.GetByIdAsync(id, new QueryOptions<Ingredient>
            {
                Includes = "ProductIngredients,ProductIngredients.Product"
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Procesa la edición del ingrediente.
        public async Task<IActionResult> Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.UpdateAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);

        }
    }
}