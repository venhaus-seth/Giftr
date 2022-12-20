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

//******************************SHOW A WISHLIST***********************************************
    [SessionCheck]
    [HttpGet("items/{UserId}")]
    public IActionResult OneWishList(int UserId)
    {
        System.Console.WriteLine("*****************" + UserId);
        MyViewModel MyModels = new MyViewModel
        {
            User = _context.Users.Include(u=>u.ItemList)
                                .Include(u=>u.Memberships)
                                .FirstOrDefault(u=>u.UserId == UserId)
        };
    return View(MyModels);
    }

//******************************ADD ITEM FORM***********************************************
    [SessionCheck]
    [HttpGet("items/new")]
    public IActionResult AddItem()
    {
        MyViewModel MyModels = new MyViewModel
        {
            User = _context.Users.Include(u=>u.ItemList)
                                .FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("UserId"))
        };
    return View(MyModels);
    }

//******************************CREATE ITEM***********************************************
    [SessionCheck]
    [HttpPost("items/create")]
    public ActionResult CreateItem(Item newItem)
    {
        if (ModelState.IsValid)
        {
            _context.Items.Add(newItem);
            _context.SaveChanges();
            return RedirectToAction("AddItem");
        }
        return View("AddItem");
    }

//******************************EDIT ITEM FORM***********************************************
    [SessionCheck]
    [HttpGet("items/{ItemId}/edit")]
    public IActionResult EditItem(int ItemId)
    {
        MyViewModel MyModels = new MyViewModel
        {
            Item = _context.Items.FirstOrDefault(i=>i.ItemId == ItemId),
            User = _context.Users.Include(u=>u.ItemList)
                                .FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("UserId"))
        };
        return View(MyModels);
    }

//******************************UPDATE ITEM***********************************************
    [SessionCheck]
    [HttpPost("items/{ItemId}/update")]
    public ActionResult UpdateItem(Item ItemToUpdate, int ItemId)
    {
        if (ModelState.IsValid)
        {
            Item? OldItem = _context.Items.FirstOrDefault(i=>i.ItemId == ItemId);
            
            OldItem.Name = ItemToUpdate.Name;
            OldItem.Description = ItemToUpdate.Description;
            OldItem.Image = ItemToUpdate.Image;
            OldItem.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("OneWishList", new {OldItem.UserId});
        }
        return View("EditItem", ItemId);
    }
}