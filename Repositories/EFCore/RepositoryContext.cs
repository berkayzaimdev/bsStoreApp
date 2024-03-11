using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Repositories.Config;

namespace Repositories.EFCore
{
    public class RepositoryContext : IdentityDbContext<User> // IdentityDbContext, içerdiği tablolarıyla beraber DB'ye yansıtılır. Generic olarak User'ı yani kullanıcıyı tanımladık
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // IdentityDbContext'e geçtiğimiz için bu sınıfın kalıtsal metodunu uygulamalıyız
            //modelBuilder.ApplyConfiguration(new BookConfig()); // Data seeding method
            //modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
