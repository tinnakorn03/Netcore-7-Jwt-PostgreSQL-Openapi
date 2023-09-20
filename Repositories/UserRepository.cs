namespace NetcoreJwtJsonbOpenapi.Repositories;

using Dapper;
using NetcoreJwtJsonbOpenapi.Entities;
using NetcoreJwtJsonbOpenapi.Helpers;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task<User> GetUser(string username);
    Task<User> GetByEmail(string email);
    Task Create(User user);
    Task Update(User user);
    Task Delete(int id);
}

public class UserRepository : IUserRepository
{
    private DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users
        """;
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<User> GetUser(string username)
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM Users WHERE Username = @username";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { username });
    }

    public async Task<User> GetByEmail(string email)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users 
            WHERE Email = @email
        """;
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
    }

    public async Task Create(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Users (Username, Title, FirstName, LastName, Email, Role, PasswordHash)
            VALUES (@Username, @Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Update(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Users 
            SET 
                Username = @Username,
                Title = @Title,
                FirstName = @FirstName,
                LastName = @LastName, 
                Email = @Email, 
                Role = @Role, 
                PasswordHash = @PasswordHash
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Users 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}