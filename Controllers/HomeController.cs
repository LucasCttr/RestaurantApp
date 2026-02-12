using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Muestra la página principal (Index).
    public IActionResult Index()
    {
        return View();
    }


    // Muestra la página de privacidad.
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // Muestra la vista de error con información del RequestId.
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
