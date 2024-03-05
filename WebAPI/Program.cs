using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
using WebAPI.Extensions;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);
// Yazdıgımız extension metodu uyguladık. Parametre olarak konfigürasyonu geçtik

builder.Services.ConfigureRepositoryManager();
// IoC'ye RepositoryManager'ı kaydetmek için yazdığımız extension metodu uyguladık

builder.Services.ConfigureServiceManager();
// IoC'ye ServiceManager'ı kaydetmek için yazdığımız extension metodu uyguladık

builder.Services.ConfigureLoggerService();
// IoC'ye LoggerManager'ı kaydet

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
