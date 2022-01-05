using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Presentation.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Library.Infrustracture.Data.SqlServer.Commands.Common;
using Library.Infrustracture.Data.SqlServer.Queries.Common;

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
            services.AddLiparServices(Configuration, nameof(Lipar), nameof(Library));

            services.AddDbContext<LibraryCommandDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("CommandConnectionString")));

            services.AddDbContext<LibraryQueryDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("QueryConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LiparOptions liparOptions)
        {
            app.AddLiparConfiguration(env, liparOptions);
        }
    }
}
