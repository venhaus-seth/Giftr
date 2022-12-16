#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Giftr.Models;
public class ValidateGroupCode
{
    [Required]
    [Display(Name = "All Gift Exchanges")]
    public int VGiftExchangeId { get; set; } 
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Group Code")]
    public string VGroupCode { get; set; } 
    
}