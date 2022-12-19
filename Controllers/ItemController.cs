using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; //SessionCheck
using Microsoft.AspNetCore.Identity; //Password Hasher
using Microsoft.EntityFrameworkCore; // .Include(), and other methods
using Giftr.Models;

namespace Giftr.Controllers;

public class ItemController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public ItemController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [SessionCheck]
    [HttpGet("Items/{UserId}")]
    public IActionResult OneWishList(int UserId)
    {
        MyViewModel MyModels = new MyViewModel
        {
            User = _context.Users.Include(u=>u.ItemList)
                                .Include(u=>u.Memberships)
                                .FirstOrDefault(u=>u.UserId == UserId)
        };
    return View(MyModels);
    }
}