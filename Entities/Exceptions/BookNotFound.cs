using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class BookNotFound : NotFoundException
    {
        public BookNotFound(int id) : base($"The book with id:{id} not found.")
        {
        }
    }
}
