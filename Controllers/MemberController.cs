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
        if (ModelState.IsValid)
        {
            //find gift exchange that was selected in form
            GiftExchange? giftExchangeInDb = _context.GiftExchanges.FirstOrDefault(c=>c.GiftExchangeId == newMember.VGiftExchangeId);

            //create member object with session-UserId and giftExchangeId from form
            Member MemberToAdd = new Member()
            {
            UserId = (int)HttpContext.Session.GetInt32("UserId"),
            GiftExchangeId = newMember.VGiftExchangeId
            };

            //verify that MemberToAdd is unique, if not render error msg and View to form 
            if (_context.Members.Any(m=>m.GiftExchangeId == MemberToAdd.GiftExchangeId && m.UserId == MemberToAdd.UserId))
            {
                ModelState.AddModelError("VGroupCode", "You are already a member of this Gift Exchange!");
                ViewBag.AllGiftExchanges = _context.GiftExchanges.OrderBy(c=>c.ExchangeDate).ToList();
                return View("MemberForm");
            }

            //verify that the groupCode is correct. If so, add member to DB
            if(newMember.VGroupCode == giftExchangeInDb.GroupCode)
            {
                _context.Members.Add(MemberToAdd);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "GiftExchange");
            }
        }
        //Return to the memberform with viewbag of all GE's
        ViewBag.AllGiftExchanges = _context.GiftExchanges.OrderBy(c=>c.ExchangeDate).ToList();
        return View("MemberForm");
    }
}