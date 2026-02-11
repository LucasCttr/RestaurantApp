using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ILogger<ProductController> logger, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.products = new Repository<Product>(context);
            this.ingredients = new Repository<Ingredient>(context);
            this.categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
            if (id == 0)
            {
                ViewBag.Operation = "Add";
                return View(new Product());

            }
            else
            {
                ViewBag.Operation = "Edit";
                return View(await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients,ProductIngredients.Ingredient, Category"
                }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = uniqueFileName;
                }
                if (product.ProductId == 0)
                {
                    ViewBag.Ingredients = await ingredients.GetAllAsync();
                    ViewBag.Categories = await categories.GetAllAsync();
                    product.CategoryId = catId;

                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient
                        {
                            IngredientId = id,
                            ProductId = product.ProductId
                        });
                    }
                    await products.AddAsync(product);
                    return RedirectToAction("index", "Product");
                }
                else
                {
                    return RedirectToAction("index", "Product");
                }
            }
            else
            {
                return View(product);
            }
        }



    }
}