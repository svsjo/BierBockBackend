//using OpenFoodDbAbfrage;

using BierBockBackend.Data;
using DataStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDatabaseContext>(); //(options => options.UseSqlite("name=ConnectionStrings:DefaultConnection"));


builder.Services.AddScoped<OpenFoodFactsDataBaseManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var scope = app.Services.CreateScope();
var foodFactsDBMgr = (OpenFoodFactsDataBaseManager)scope.ServiceProvider.GetService(typeof(OpenFoodFactsDataBaseManager));
foodFactsDBMgr.Insert();


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

