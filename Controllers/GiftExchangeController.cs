using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; //SessionCheck
using Microsoft.EntityFrameworkCore; // .Include(), and other methods
using Giftr.Models;

namespace Giftr.Controllers;

public class GiftExchangeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    
    public GiftExchangeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

//******************************DASHBOARD***********************************************
    [SessionCheck]
    [HttpGet("giftexchange/dashboard")]
    public IActionResult Dashboard()
    {
        MyViewModel MyModels = new MyViewModel
        {
            AllGiftExchanges = _context.GiftExchanges.OrderBy(c=>c.ExchangeDate).ToList()
        };

        return View(MyModels);
    }

//******************************GIFT EXCHANGE FORM***********************************************
    [SessionCheck]
    [HttpGet("giftexchanges/new")]
    public IActionResult GiftExchangeForm()
    {
        return View();
    }

//******************************CREATE GIFT EXCHANGE***********************************************
    [SessionCheck]
    [HttpPost("giftexchanges/create")]
    public IActionResult CreateGiftExchange(GiftExchange newGiftExchange)
    {
        if (ModelState.IsValid)
        {
            _context.GiftExchanges.Add(newGiftExchange);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        return View("GiftExchangeForm");
    }
}