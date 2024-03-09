using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParams bookParams, bool trackChanges)
        {
            if (!bookParams.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithMetaData = await _manager
                .Book   
                .GetAllBooksAsync(bookParams, trackChanges);
            return (_mapper.Map<IEnumerable<BookDto>>(booksWithMetaData), booksWithMetaData.MetaData);
            // mapleme işleminde elde etmek istediğimiz tip, angle parantez içine alınır. Mapleyeceğimiz tip ise normal parantezin içinde yer alır.
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookAndCheckExist(id, trackChanges);

            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }
        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = await GetOneBookAndCheckExist(id, trackChanges);

            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }
        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookAndCheckExist(id, trackChanges);
            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }
        private async Task<Book> GetOneBookAndCheckExist(int id, bool trackChanges)
        {
            // check entity 
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            return entity;
        }
    }
}
