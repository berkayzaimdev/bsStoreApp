using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }
        public async Task<PagedList<Book>> GetAllBooksAsync(BookParams bookParams, bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .FilterBooks(bookParams.MinPrice, bookParams.MaxPrice)
                .ToListAsync();

            return PagedList<Book>.ToPagedList(books, bookParams.PageNumber, bookParams.PageSize);
        }
        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreateOneBook(Book book) => Create(book);
        public void UpdateOneBook(Book book) => Update(book);
        public void DeleteOneBook(Book book) => Delete(book);
    }
}
