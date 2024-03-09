using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
using Services.Contracts;
using WebAPI.Extensions;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

builder.Services.AddControllers(config =>    
{
    config.RespectBrowserAcceptHeader = true;
    // API'leri content negotiation'a açık hale getirir. Default olarak bu seçenek false'tur.
    config.ReturnHttpNotAcceptable = true;
    // API'lerin response olarak 406 kodu dönmesine izin verir. Bu kod content negotiaton yapmaya çalıştığımızı, fakat başarısız olduğumuzu ifade eder.
}
)
.AddCustomCsvFormatter()
.AddXmlDataContractSerializerFormatters() // API'lerin response olarak XML formatında veri dönmesine izin verdik
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
.AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

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

builder.Services.AddAutoMapper(typeof(Program));
// Tek satırda çağırabildiğimiz için extension olarak yazmaya gerek duymadık

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();

app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
