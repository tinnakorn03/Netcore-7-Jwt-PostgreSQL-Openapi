namespace NetcoreJwtJsonbOpenapi.Entities;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }

    // Override the ToString() method
    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, FirstName: {FirstName}, LastName: {LastName}, Username: {Username}, Email: {Email}, Role: {Role}, PasswordHash: {PasswordHash}";
    }
}