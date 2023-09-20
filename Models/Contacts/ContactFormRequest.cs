namespace NetcoreJwtJsonbOpenapi.Models.Contact;

using System.ComponentModel.DataAnnotations;

public class ContactFormRequest
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

   [Required]
    public string? Message { get; set; }
 
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; } 
} 