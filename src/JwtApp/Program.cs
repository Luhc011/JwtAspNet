using JwtApp;
using JwtApp.Extensions;
using JwtApp.Models;
using JwtApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenServices>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        //ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("admin", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/login", (TokenServices service) =>
{
    var user = new User
    (
        1,
        "",
        "lcs@example.com",
        "lcs-aoa",
        "123456",
        new[] { "admin", "user" }
    );
    return service.Create(user);
});

app.MapGet("/restrito", (ClaimsPrincipal user) => new
{
    Id = user.Id(),
    Name = user.Name(),
    Email = user.Email(),
    Image = user.Image()
}).RequireAuthorization();

app.MapGet("/admin", () => "Acesso admin permitdo").RequireAuthorization("admin");
app.MapGet("/user", () => "Acesso user permitdo").RequireAuthorization("user");

app.Run();
