namespace NetcoreJwtJsonbOpenapi.Helpers;

using System.Data; 
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

public class DataContext
{
    private readonly AppSettings _appSettings;

    public DataContext(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public IDbConnection CreateConnection()
    { 
        return new NpgsqlConnection(_appSettings?.ConnectionStrings?.DefaultConnection);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = _appSettings?.ConnectionStrings?.DefaultConnection;
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_appSettings?.ConnectionStrings?.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_appSettings?.ConnectionStrings?.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sqlUsers = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Username VARCHAR,
                    Title VARCHAR,
                    FirstName VARCHAR,
                    LastName VARCHAR,
                    Email VARCHAR,
                    Role INTEGER,
                    PasswordHash VARCHAR
                );";

            var sqlContacts = @"
                CREATE TABLE IF NOT EXISTS Contacts (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(255),
                    Email VARCHAR(255),
                    Message TEXT,
                    Date DATE
                );";

            await connection.ExecuteAsync(sqlUsers);
            await connection.ExecuteAsync(sqlContacts);
        }
    }

}