using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; //SessionCheck
using Microsoft.EntityFrameworkCore; // .Include(), and other methods
using Giftr.Models;

namespace Giftr.Controllers;

public class MemberController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    
    public MemberController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

//******************************GIFT EXCHANGE FORM***********************************************
    [SessionCheck]
    [HttpGet("members/new")]
    public IActionResult MemberForm()
    {
        ViewBag.AllGiftExchanges = _context.GiftExchanges.OrderBy(c=>c.ExchangeDate).ToList();
        return View();
    }

    [SessionCheck]
    [HttpPost("members/create")]
    public IActionResult CreateMember(Member newMember)
    {
        // if (ModelState.IsValid)
        // {
        //     ModelState.AddModelError("VGroupCode", "Invalid Group Code");
        //     return View("MemberForm");
        // }

        GiftExchange? giftExchangeInDb = _context.GiftExchanges.FirstOrDefault(c=>c.GiftExchangeId == newMember.VGiftExchangeId);
            
        if(newMember.VGroupCode == giftExchangeInDb.GroupCode)
        {
            _context.Members.Add(newMember);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        return View("MemberForm");
    }
}