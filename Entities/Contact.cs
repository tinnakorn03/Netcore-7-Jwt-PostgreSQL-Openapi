namespace NetcoreJwtJsonbOpenapi.Entities;

using System.Text.Json.Serialization;

public class Contact
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; } 
    public int? Phone_Number { get; set; } 
    public string? Message { get; set; }
    public DateTime? Date { get; set; } 
 
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Email: {Email}, Phone_Number: {Phone_Number}, Message: {Message}, Date: {Date}";
    }
}