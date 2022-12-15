using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace Giftr.Models;
public class Member
{
    [Key]
    public int MemberId {get;set;}
    public int UserId {get;set;}
    public int GiftExchangeId {get;set;}
    public User? User {get;set;}
    public GiftExchange? GiftExchange {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
	public DateTime UpdatedAt { get; set; } = DateTime.Now;
}