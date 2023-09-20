namespace NetcoreJwtJsonbOpenapi.Models.Contact;

using NetcoreJwtJsonbOpenapi.Entities;

public class ContactFormResponse
{ 
    public string Status { get; set; }
    public string? Message { get; set; }
    
 
    public ContactFormResponse(Contact contact,string status)
    { 
        Message = contact.Message; 
        Status = status; 
    }
}