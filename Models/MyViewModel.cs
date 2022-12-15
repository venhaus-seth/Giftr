#pragma warning disable CS8618
namespace Giftr.Models;
public class MyViewModel
{
    public User? User {get;set;}
    public List<User> AllUsers {get;set;}
    public GiftExchange? GiftExchange {get;set;}
    public List<GiftExchange> AllGiftExchanges {get;set;}
    public Item? Item {get;set;}
    public List<Item> AllItems {get;set;}
    public Member? Member {get;set;}
    public List<Member> AllMembers {get;set;}
}