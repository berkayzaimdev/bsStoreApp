using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services;
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
    config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
}
)
.AddXmlDataContractSerializerFormatters() // API'lerin response olarak XML formatında veri dönmesine izin verdik
.AddCustomCsvFormatter()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
//.AddNewtonsoftJson();

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

builder.Services.ConfigureActionFilters();

builder.Services.ConfigureCors();
// CORS konfigürasyonu

builder.Services.ConfigureDataShaper();
// Data Shaper konfigürasyonu

builder.Services.AddCustomMediaTypes();
// Özel dosya tiplerine izin verdiğimiz konfigürasyon

builder.Services.ConfigureVersioning();
// Versiyonlama için konfigürasyon

builder.Services.ConfigureResponseCaching();
// Caching için konfigürsayon

builder.Services.ConfigureHttpCacheHeaders();
// Marvin cache paketi için konfigürasyon

builder.Services.ConfigureRateLimitingOptions();
// Hız sınırlama için konfigürasyon

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

builder.Services.AddScoped<IBookLinks, BookLinks>();

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

app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

app.UseResponseCaching(); // Microsoft'un önerisi CORS'tan sonra kullanmak

app.UseAuthentication(); // önce oturum açma
app.UseAuthorization();  // sonra yetkilendirme

app.MapControllers();

app.Run();
