using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _manager.Book.GetAllBooks(false);
            return Ok(books);
        }

        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = _manager.Book.GetOneBookById(id, false);

            if (book is null)
                return NotFound(); //404

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); // 400 

                _manager.Book.CreateOneBook(book);
                _manager.Save();
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book)
        {
            try 
            {
                // check book?
                var entity = _manager
                    .Book
                    .GetOneBookById(id, true);

                if (entity is null)
                    return NotFound(); // 404

                // check id
                if (id != book.Id)
                    return BadRequest(); // 400

                entity.Title = book.Title;
                entity.Price = book.Price;

                _manager.Save();

                return Ok(book);
            }
            catch(Exception exc)
            {
                throw new Exception(exc.Message);
            } 
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAllBooks([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _manager
                    .Book
                    .GetOneBookById(id, true);

                if (entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });  // 404

                _manager.Book.DeleteOneBook(entity);
                _manager.Save();

                return NoContent();
            }
            catch(Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
    }
}
