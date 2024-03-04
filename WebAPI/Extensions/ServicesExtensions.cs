using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;

namespace WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        // Extension sınıfı ve metodu yazmak için gerekli olan parametreyi geçtik
        // Hangi nesneye extension metot yazmak istiyorsak o sınıfı this ile işaretleriz.
        // Bu nesne IServiceCollection olabilir, başka bir interface de olabilir.
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    }
}
