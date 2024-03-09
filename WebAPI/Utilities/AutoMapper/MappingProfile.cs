using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace WebAPI.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>();
            CreateMap<Book, BookDto>(); // Book tipinden -> BookDto tipine mapleme yapabilmemiz için bu metodu yazdık. Az önce BookManager'da yaptığımız düzenlemeyi anlamlı kılmak için bunu yaptık
            CreateMap<BookDtoForInsertion, Book>();
        }
    }
}
