//using OpenFoodDbAbfrage;

using BierBockBackend.Data;
using DataStorage;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Timers;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


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

app.UseAuthorization();

app.MapControllers();

app.Run();

