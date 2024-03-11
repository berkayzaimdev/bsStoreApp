using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record LinkParams
    {
        public BookParams BookParams { get; init; }
        public HttpContext HttpContext { get; init; }
    }
}
