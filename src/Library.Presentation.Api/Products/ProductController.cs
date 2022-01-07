using Lipar.Infrastructure.Tools.Utilities.Services;
using Lipar.Presentation.Api.Controllers;
using Library.Core.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Library.Infrustracture.Tools.Cache.Redis;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using StackExchange.Redis;

namespace Library.Presentation.Api.Products
{
    //[ApiVersion("1.0")]
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> logger;
        private readonly IJson json;
        private readonly ICacheProvider cache;

        public ProductController(ILogger<ProductController> logger, IJson json, ICacheProvider cache)
        {
            this.logger = logger;
            this.json = json;
            this.cache = cache;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var options = new DistributedCacheEntryOptions();

            //options.SetAbsoluteExpiration(System.DateTimeOffset.Now.AddSeconds(60));

            var value = await cache.Get<CreateProductCommand>("key01");

            await cache.Set("key01", command, options);


            return await SendAsync(command, HttpStatusCode.Created);
        }

    }


}
