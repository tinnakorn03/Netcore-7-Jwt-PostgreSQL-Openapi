using System.Text.Json.Serialization;
using NetcoreJwtJsonbOpenapi.Authorization;
using NetcoreJwtJsonbOpenapi.Helpers;
using NetcoreJwtJsonbOpenapi.Services;
using NetcoreJwtJsonbOpenapi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); 
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Tasks API",
        Description = "Demo for test",
        TermsOfService = new Uri("https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0&source=recommendations#openapi-specification-openapijson"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Contact us",
            Url = new Uri("https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0&source=recommendations#openapi-specification-openapijson"),
            Email = "aram@example.com"
        }, 
        License = new Microsoft.OpenApi.Models.OpenApiLicense 
        { 
            Name = "License", 
            Url = new Uri("https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0&source=recommendations#openapi-specification-openapijson") 
        }
    }); 
    // Add this block to your existing Swagger config
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine($"Dev: {app.Environment.IsDevelopment()}");  // This will print to the console
Console.WriteLine($"UAT: {app.Environment.IsStaging()}");  // This will print to the console
Console.WriteLine($"PROD: {app.Environment.IsProduction()}");  // This will print to the console

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
