using NetcoreJwtJsonbOpenapi.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var isAllowAnonymous = context.MethodInfo.GetCustomAttributes(true)
            .OfType<NetcoreJwtJsonbOpenapi.Authorization.AllowAnonymousAttribute>().Any(); // Make sure to use your custom namespace

        if (isAllowAnonymous)
        {
            // If "AllowAnonymous" is present, then remove the security definitions
            operation.Security = new List<OpenApiSecurityRequirement>();
        }
        else
        {
            // If not, add the security definitions
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}},
                        new List<string>()
                    }
                }
            };
        }
    }
}
