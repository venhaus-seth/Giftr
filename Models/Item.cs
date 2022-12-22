using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace Giftr.Models;
public class Item
{
    [Key]
    public int ItemId {get;set;}
    [Required]
    [MinLength(2)]
    public string Name {get;set;}
    [Required]
    [Range(0,25, ErrorMessage = "description cannot be longer the 25 characters")]
    public string Description {get;set;}
    public string Image {get;set;}
    public int UserId {get;set;}
    public User? User {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
	public DateTime UpdatedAt { get; set; } = DateTime.Now;
}