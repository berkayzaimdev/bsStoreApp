using Entities.Dtos;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controller
{
    // [ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/{v:apiversion}/books")]
    // [ResponseCache(CacheProfileName = "5mins")]
    // [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)] // Marvin cache paketi için özel attribute
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [Authorize]
        // [HttpHead] // Header'dan veri döndürmek için bu notasyonu kullanabiliriz
        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        // [ResponseCache(Duration = 60)] // attribute olarak cache tanımlaması
        public async Task<IActionResult> GetAllBooksAsync([FromQuery]BookParams bookParams)
        {
            var linkParams = new LinkParams()
            {
                BookParams = bookParams,
                HttpContext = HttpContext
            };

            var result = await _manager
                .BookService
                .GetAllBooksAsync(linkParams, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));
            return result.linkResponse.HasLinks ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntities);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager
                .BookService
                .GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            //if (bookDto is null)
            //    return BadRequest(); // 400 

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);
            // ActionFilter kullandığımız için bu kod bloğuna gerek kalmadı

            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book);  // CreatedAtRoute()
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id,
            [FromBody] BookDtoForUpdate bookDto)
        {
            //if (bookDto is null)
            //    return BadRequest(); // 400

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState); // 422
            // ActionFilter kullandığımız için bu kod bloğuna gerek kalmadı

            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);

            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            try
            {
                await _manager.BookService.DeleteOneBookAsync(id, false);

                return NoContent();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
            return NoContent();
        }
    }
}
