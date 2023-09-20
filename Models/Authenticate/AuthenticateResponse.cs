namespace NetcoreJwtJsonbOpenapi.Models;

using NetcoreJwtJsonbOpenapi.Entities;

public class AuthenticateResponse : User
{ 
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Title = user.Title;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Email = user.Email;
        Role = user.Role;
        Token = token;
    }
}