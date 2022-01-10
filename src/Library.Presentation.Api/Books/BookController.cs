using Lipar.Presentation.Api.Controllers;
using Library.Core.Application.Books.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Library.Core.Application.Books.Queries;
using System;
using System.Diagnostics;
using Hangfire;
using Library.Infrustracture.Tools.Cache.Redis;
using Lipar.Infrastructure.Tools.Utilities.Services;
using System.Collections.Generic;
using Library.Core.Domain.Books.Models;
using Lipar.Core.Application.Common;
using System.Threading;
using static Library.Core.Application.Books.Commands.CreateBulkBookCommand;

namespace Library.Presentation.Api.Books
{
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class BookController : BaseController
    {
        private readonly ILogger<BookController> logger;
        private readonly ICacheProvider cache;
        private readonly IUserInfo user;
        private readonly IBackgroundJobClient backgroundJob;

        public BookController(ILogger<BookController> logger, ICacheProvider cache, IUserInfo user, IBackgroundJobClient backgroundJob)
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

            backgroundJob.Enqueue<CreateBulkBookCommandHandler>(m => 
                m.Handle(new CreateBulkBookCommand { Key = command.Key }, default(CancellationToken)));

            return result;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            return await SendAsync(command, HttpStatusCode.Created);
        }
    }


}
