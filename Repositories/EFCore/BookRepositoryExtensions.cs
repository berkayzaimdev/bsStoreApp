using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
            =>
            books.Where(book => 
            (book.Price) >= minPrice &&
            (book.Price) <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
            =>
            books.Where(book => book.Title
            .ToLower()
            .Contains(searchTerm.Trim().ToLower()));
    }
}
