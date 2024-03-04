using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using WebAPI.Extensions;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);
// Yazdıgımız extension metodu uyguladık. Parametre olarak konfigürasyonu geçtik

builder.Services.ConfigureRepositoryManager();
// IoC'ye RepositoryManager'ı kaydetmek için yazdığımız extension metodu uyguladık

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
