namespace NetcoreJwtJsonbOpenapi.Authorization;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetcoreJwtJsonbOpenapi.Entities;
using NetcoreJwtJsonbOpenapi.Helpers;
using Newtonsoft.Json; 

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public (int? UserId, User? UserInfo) ValidateJwtToken(string? token);
}

public class JwtUtils : IJwtUtils
{
    private readonly AppSettings _appSettings;

    public JwtUtils(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        if (string.IsNullOrEmpty(_appSettings.Secret))
            throw new Exception("JWT secret not configured");
    }

    public string GenerateJwtToken(User user)
    {
        // Serialize user object to JSON
        var userJson = JsonConvert.SerializeObject(user);

        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            { 
                new Claim("id", user.Id.ToString()),
                new Claim("user", userJson)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public (int? UserId, User? UserInfo) ValidateJwtToken(string? token)
    {
        if (token == null)
            return (null, null);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            var user = JsonConvert.DeserializeObject<User>(jwtToken.Claims.First(x => x.Type == "user").Value);

            return (userId, user);
        }
        catch
        { 
            return (null, null);
        }
    }
}