using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private  Repository<Order> _orders;
        private  Repository<Product> _products;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;


        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrderController> logger)
        {
            _orders = new Repository<Order>(context);
            _products = new Repository<Product>(context);
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };

            var product = model.Products.FirstOrDefault(p => p.ProductId == prodId);
            if (product != null)
            {
                var orderItem = model.OrderItems.FirstOrDefault(oi => oi.ProductId == prodId);
                if (orderItem != null)
                {
                    orderItem.Quantity += prodQty;
                }
                else
                {
                    model.OrderItems.Add(new OrderItemViewModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Quantity = prodQty,
                        Price = product.Price
                    });
                }
                model.TotalAmount = model.OrderItems.Sum(oi => oi.Quantity * oi.Price);
                HttpContext.Session.Set("OrderViewModel", model);
            }

            return View("Create", model);
        }
    }
}