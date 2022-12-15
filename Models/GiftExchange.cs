using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace Giftr.Models;
public class GiftExchange
{
    [Key]
    public int GiftExchangeId {get;set;}

    [Required]
    [MinLength(2)]
    public string Name {get;set;}
    [Required]
    [DataType(DataType.Password)]
    [MinLength(3, ErrorMessage = "GroupCode must be at least 3 characters long")]
    public string GroupCode {get;set;}
    [Required]
    [Range(0,10000)]
    public int Budget {get;set;}
    [Required]
    [FutureDate]
    public DateTime ExchangeDate {get;set;}
    [Required]
    public string Details {get;set;}
    [Required]
    public int UserId {get;set;}
	public User? Admin {get;set;}
    public List<Member> MemberList {get;set;} = new List<Member>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
	public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Date is required!");
        }

		DateTime WedDate = Convert.ToDateTime(value);
		if(DateTime.Compare(WedDate, DateTime.Now) <= 0)
        {
            return new ValidationResult("Project date must be in the future!");
        } else {
            return ValidationResult.Success;
        }
    }
}
