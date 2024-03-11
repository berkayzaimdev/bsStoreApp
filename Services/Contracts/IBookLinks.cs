using Entities.Dtos;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLink(IEnumerable<BookDto> booksDto,
            string fields, HttpContext httpContext);
    }
}
