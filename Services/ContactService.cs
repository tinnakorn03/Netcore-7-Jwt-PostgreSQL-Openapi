namespace NetcoreJwtJsonbOpenapi.Services;
using AutoMapper; 
using NetcoreJwtJsonbOpenapi.Entities; 
using NetcoreJwtJsonbOpenapi.Repositories; 
using NetcoreJwtJsonbOpenapi.Models.Contact;
using MailKit.Net.Smtp;
using MimeKit;
public interface IContactService
{ 
    Task<ContactFormResponse?> CreateForm(ContactFormRequest model); 
}


public class ContactService : IContactService
{ 
    private IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<ContactFormResponse?> CreateForm(ContactFormRequest model)
    { 
        var contact = _mapper.Map<Contact>(model); 
        await _contactRepository.Create(contact);

        // Initialize the message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("No Reply", "demofortest0@gmail.com"));
        message.To.Add(new MailboxAddress("Recipient", model.Email));
        message.Subject = "Contact Created";
        message.Body = new TextPart("plain") { Text = model.Message };

        // Initialize the SMTP client
        using var client = new SmtpClient(); 
        await client.ConnectAsync("smtp.gmail.com", 587, false); 
        await client.AuthenticateAsync("demofortest0@gmail.com", "aawe foux xgpd xysg");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        return new ContactFormResponse(contact,"successfully");
    }
}