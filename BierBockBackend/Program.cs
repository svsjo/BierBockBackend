//using OpenFoodDbAbfrage;

using BierBockBackend.Data;
using DataStorage;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("token");
    logging.RequestHeaders.Add("Referer");
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.RequestHeaders.Add("sec-ch-ua-platform");
    logging.RequestHeaders.Add("sec-fetch-site");
    logging.RequestHeaders.Add("sec-fetch-mode");
    logging.RequestHeaders.Add("sec-fetch-dest");
    logging.RequestHeaders.Add("sec-ch-ua-mobile");
});

builder.Services.AddDbContext<AppDatabaseContext>(); //(options => options.UseSqlite("name=ConnectionStrings:DefaultConnection"));


builder.Services.AddScoped<OpenFoodFactsDataBaseManager>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/security/getMessage", () => "Hello World!").RequireAuthorization();



app.MapPost("/security/createToken",
    [AllowAnonymous] (JAuthUser user) =>
    {
        if (user.Name == "Max Mustermann" && user.Passwort == "Password123")
        {
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (builder.Configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);
        }
        return Results.Unauthorized();
    });


void FillDbFromApi()
{
    var scope = app.Services.CreateScope();
    var foodFactsDbMgr =
        (OpenFoodFactsDataBaseManager)scope.ServiceProvider.GetService(typeof(OpenFoodFactsDataBaseManager))!;
    foodFactsDbMgr?.Insert();
    foodFactsDbMgr?.InitBasicUserData();
}

var timer = new System.Timers.Timer()
{
    Interval = TimeSpan.FromDays(7).TotalMilliseconds
};
timer.Elapsed += delegate
{
    FillDbFromApi();
};
timer.Start();

Task.Run(FillDbFromApi);

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

record JAuthUser(string Name, string Passwort, string Email);