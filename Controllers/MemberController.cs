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

//******************************JOIN GIFT EXCHANGE FORM***********************************************
    [SessionCheck]
    [HttpGet("members/new")]
    public IActionResult MemberForm()
    {
        ViewBag.AllGiftExchanges = _context.GiftExchanges.OrderBy(c=>c.ExchangeDate).ToList();
        return View();
    }

//******************************CREATE MEMBER***********************************************
    [SessionCheck]
    [HttpPost("members/create")]
    public IActionResult CreateMember(ValidateGroupCode newMember)
    {

        GiftExchange? giftExchangeInDb = _context.GiftExchanges.FirstOrDefault(c=>c.GiftExchangeId == newMember.VGiftExchangeId);

        Member MemberToAdd = new Member()
        {
        UserId = (int)HttpContext.Session.GetInt32("UserId"),
        GiftExchangeId = newMember.VGiftExchangeId
        };

        if(newMember.VGroupCode == giftExchangeInDb.GroupCode)
        {
            _context.Members.Add(MemberToAdd);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "GiftExchange");
        }
        return View("MemberForm");
    }
}