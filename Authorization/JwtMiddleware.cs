namespace NetcoreJwtJsonbOpenapi.Authorization;

using NetcoreJwtJsonbOpenapi.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var (userId, UserInfo) = jwtUtils.ValidateJwtToken(token);
        // Console.WriteLine($"JwtMiddleware > Invoke: {userId}"); 

        if (userId != null)
        {
            // context.Items["User"] = UserInfo;
            var response = await userService.GetById(userId.Value); 
            context.Items["User"] = response;
        } 

        await _next(context);
    }
}