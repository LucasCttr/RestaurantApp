using Microsoft.AspNetCore.Identity;

namespace RestaurantApp.Models;

public class ApplicationUser : IdentityUser
{
    ICollection<Order> Orders { get; set; }
}