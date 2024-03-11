﻿using Entities.Dtos;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;

namespace Services
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLink(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext)
        {
            var shapedBooks = ShapeData(booksDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(booksDto, fields, httpContext, shapedBooks);
            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList = booksDto.ToList();

            for(int i=0;i<bookDtoList.Count();i++) 
            {
                var booksLinks = CreateForBook(httpContext, bookDtoList[i], fields);
                shapedBooks[i].Add("Links", booksLinks);
            }
            
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            return new LinkResponse { HasLinks = true, LinkedEntities = bookCollection };
        }

        private object CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>();
            {
                new Link("a1", "b1", "c1");
                new Link("a2", "b2", "c2");
            };

            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse()
            {
                ShapedEntities = shapedBooks
            };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"]; // ValidateMediaTypeAttribute action filter'ında bu atamayı yapmıştık

            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<BookDto> booksDto, string fields)
        {
            return _dataShaper
                .ShapeData(booksDto, fields)
                .Select(b => b.Entity)
                .ToList();
        }
    }
}
