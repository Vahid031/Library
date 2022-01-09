using Lipar.Infrastructure.Tools.Utilities.Services;
using Lipar.Presentation.Api.Controllers;
using Library.Core.Application.Books.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Library.Infrustracture.Tools.Cache.Redis;
using Library.Core.Application.Books.Queries;
using System.IO.Compression;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Library.Presentation.Api.Books
{
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class BookController : BaseController
    {
        private readonly ILogger<BookController> logger;
        private readonly IJson json;
        private readonly ICacheProvider cache;
        private readonly IHostingEnvironment env;

        public BookController(ILogger<BookController> logger, IJson json, ICacheProvider cache, IHostingEnvironment env)
        {
            this.logger = logger;
            this.json = json;
            this.cache = cache;
            this.env = env;
        }

        [HttpGet("sampleFile")]
        public async Task<IActionResult> SampleFile([FromQuery] SampleFileQuery query)
        {
            Stream memoryStream = await mediator.Send(query);

            return new FileStreamResult(memoryStream, "application/octet-stream")
            {
                FileDownloadName = "Sample.xlsx"
            };
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadBookCommand command)
        {
            return await SendAsync(command, HttpStatusCode.Created);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            return await SendAsync(command, HttpStatusCode.Created);
        }

    }


}
