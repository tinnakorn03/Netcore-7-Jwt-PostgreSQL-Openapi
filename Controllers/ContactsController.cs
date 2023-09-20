namespace NetcoreJwtJsonbOpenapi.Controllers;

using Microsoft.AspNetCore.Mvc;
using NetcoreJwtJsonbOpenapi.Authorization;
using NetcoreJwtJsonbOpenapi.Models.Contact;
using NetcoreJwtJsonbOpenapi.Services;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }
  
    [HttpPost("contact")]
    public async Task<IActionResult> Create(ContactFormRequest model)
    {
        await _contactService.CreateForm(model);
        return Ok(new { message = "Contact created" });
    } 
 
}
