namespace NetcoreJwtJsonbOpenapi.Repositories;

using Dapper;
using NetcoreJwtJsonbOpenapi.Entities;
using NetcoreJwtJsonbOpenapi.Helpers;

public interface IContactRepository
{ 
    Task Create(Contact model); 
}

public class ContactRepository : IContactRepository
{
    private DataContext _context;

    public ContactRepository(DataContext context)
    {
        _context = context;
    } 

    public async Task Create(Contact model)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Contacts (Name, Email, Message, Date)
            VALUES (@Name, @Email, @Message, @Date)
        """;
        await connection.ExecuteAsync(sql, model);
    }
 
}