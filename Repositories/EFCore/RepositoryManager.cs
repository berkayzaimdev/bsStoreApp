using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IBookRepository> _bookRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
        }

        // public IBookRepository Book => new BookRepository(_context);
        // IoC'ye sadece RepositoryManager'ın kaydını yapabilmek için böyle bir kullanıma gittik.
        // Dilersek tek tek her repository için constructor'da geçebilirdik, veya AutoFac kullanabilirdik
        // Bunu da farklı bir kullanım olarak görmüş olalım

        public IBookRepository Book => _bookRepository.Value;
        // Sadece kullanıldığı anda getirmek için Lazy Loading yaparak gereksiz kaynak kullanımını azalttık

        public async Task SaveAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}
