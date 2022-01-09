using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Presentation.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Library.Infrustracture.Data.SqlServer.Commands.Common;
using Library.Infrustracture.Data.SqlServer.Queries.Common;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Library.Infrustracture.Tools.Cache.Redis;

namespace Library.Presentation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLiparServices(Configuration, nameof(Lipar), $"{nameof(Library)}.");

            services.AddDbContext<LibraryCommandDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("CommandConnectionString")));

            services.AddDbContext<LibraryQueryDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("QueryConnectionString")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("CacheConnectionString");
                options.InstanceName = "master";
            });

            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());

            services.AddScoped<ICacheProvider, CacheProvider>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LiparOptions liparOptions)
        {
            app.AddLiparConfiguration(env, liparOptions);
        }
    }
}
