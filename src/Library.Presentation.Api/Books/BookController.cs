using Lipar.Presentation.Api.Controllers;
using Library.Core.Application.Books.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Hangfire;
using Library.Infrustracture.Tools.Cache.Redis;
using System.Threading;
using Lipar.Core.Contract.Services;
using Library.Core.Contract.Books.Queries;
using Library.Core.Contract.Books.Commands;
using static Library.Core.Application.Books.Commands.CreateBulkBookCommand;

namespace Library.Presentation.Api.Books
{
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class BookController : BaseController
    {
        private readonly ILogger<BookController> logger;
        private readonly ICacheProvider cache;
        private readonly IUserInfoService user;
        private readonly IBackgroundJobClient backgroundJob;

        public BookController(ILogger<BookController> logger, ICacheProvider cache, IUserInfoService user, IBackgroundJobClient backgroundJob)
        {
            this.logger = logger;
            this.cache = cache;
            this.user = user;
            this.backgroundJob = backgroundJob;
        }
        [HttpGet("sampleFile")]
        public async Task<IActionResult> SampleFile([FromQuery] SampleFileQuery query)
        {
            return new FileStreamResult(await mediator.Send(query), "application/octet-stream")
            {
                FileDownloadName = "Sample.xlsx"
            };
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadBookCommand command)
        {
            var sw = Stopwatch.StartNew();

            command.Key = DateTime.UtcNow.ToString($"yyyyMMddHHmmss");

            var result = await SendAsync(command, HttpStatusCode.Created);

            sw.Stop();
            logger.LogInformation($"Upload data finished at {sw.ElapsedMilliseconds / 1000} seconds");

            backgroundJob.Schedule<CreateBulkBookCommandHandler>(m => 
                m.Handle(new CreateBulkBookCommand { Key = command.Key }, default(CancellationToken)), DateTime.Now);

            return result;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            return await SendAsync(command, HttpStatusCode.Created);
        }
    }


}
