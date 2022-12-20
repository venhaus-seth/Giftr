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
            AllGiftExchanges = _context.GiftExchanges.Include(ge=>ge.MemberList)
                                                    .ThenInclude(m=>m.User)
                                                    .OrderBy(ge=>ge.ExchangeDate)
                                                    .ToList()
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

//******************************DISPLAY GIFT EXCHANGE and MEMBERS***********************************************
    [SessionCheck]
    [HttpGet("giftexchanges/{GiftExchangeId}")]
    public IActionResult OneGiftExchange(int GiftExchangeId)
    {
        MyViewModel MyModels = new MyViewModel
        {
            GiftExchange = _context.GiftExchanges.Include(ge=>ge.MemberList)
                                        .ThenInclude(m=>m.User)
                                        .FirstOrDefault(ge=>ge.GiftExchangeId == GiftExchangeId)
        };

        return View(MyModels);
    }

    //******************************EDIT GIFT EXCHANGE***********************************************
    [SessionCheck]
    [HttpGet("giftexchanges/{GiftExchangeId}/edit")]
    public IActionResult EditGiftExchange(int GiftExchangeId)
    {
        GiftExchange? GEInDb = _context.GiftExchanges.SingleOrDefault(ge=>ge.GiftExchangeId == GiftExchangeId);
        return View(GEInDb);
    }

    //******************************UPDATE GIFT EXCHANGE***********************************************
    [SessionCheck]
    [HttpPost("giftexchanges/{GiftExchangeId}/update")]
    public IActionResult UpdateGiftExchange(GiftExchange GEToUpdate, int GiftExchangeId)
    {
        System.Console.WriteLine(GEToUpdate.Name);
        if (ModelState.IsValid)
        {
            System.Console.WriteLine("test - after IsValid");
            GiftExchange? OldGE = _context.GiftExchanges.FirstOrDefault(ge=>ge.GiftExchangeId == GiftExchangeId);

            OldGE.Name = GEToUpdate.Name;
            OldGE.GroupCode = GEToUpdate.GroupCode;
            OldGE.Budget = GEToUpdate.Budget;
            OldGE.ExchangeDate = GEToUpdate.ExchangeDate;
            OldGE.Details = GEToUpdate.Details;
            OldGE.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("OneGiftExchange", new {GiftExchangeId});
        }
        return View("EditGiftExchange", new {GiftExchangeId});
    }
}