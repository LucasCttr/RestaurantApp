using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        private readonly AppDbContext _context;

        public ProductController(ILogger<ProductController> logger, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.products = new Repository<Product>(context);
            this.ingredients = new Repository<Ingredient>(context);
            this.categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
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
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients,ProductIngredients.Ingredient, Category"
                });
                ViewBag.Operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
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
                    product.ProductIngredients = new List<ProductIngredient>();
                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients.Add(new ProductIngredient
                        {
                            IngredientId = id
                        });
                    }
                    await products.AddAsync(product);
                    return RedirectToAction("index", "Product");
                }
                else
                {
                    var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product>
                    {
                        Includes = "ProductIngredients"
                    });
                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Product not found");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = catId;

                    // Actualizar imagen si se sube una nueva
                    if (product.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }
                        existingProduct.ImageUrl = uniqueFileName;
                    }

                    existingProduct.ProductIngredients?.Clear();
                    foreach (int id in ingredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient
                        {
                            IngredientId = id,
                            ProductId = existingProduct.ProductId
                        });
                    }
                    try
                    {
                        await products.UpdateAsync(existingProduct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating product");
                        ModelState.AddModelError("", "An error occurred while updating the product.");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("index", "Product");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients"
                });
                if (product != null)
                {
                    // Remove related ProductIngredients
                    if (product.ProductIngredients != null)
                    {
                        foreach (var pi in product.ProductIngredients)
                        {
                            _context.ProductIngredients.Remove(pi);
                        }
                    }
                    await products.DeleteAsync(id);
                }
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                ModelState.AddModelError("", "An error occurred while deleting the product.");
                return RedirectToAction("index");
            }
        }
    }
}