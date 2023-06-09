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
using Hangfire;

namespace Library.Presentation.Api;

public static class HostingExtension
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddLiparServices(builder.Configuration, nameof(Lipar), $"{nameof(Library)}.");

        builder.Services.AddDbContext<LibraryCommandDbContext>(
            c => c.UseSqlServer(builder.Configuration.GetConnectionString("CommandConnectionString")));

        builder.Services.AddDbContext<LibraryQueryDbContext>(
            c => c.UseSqlServer(builder.Configuration.GetConnectionString("QueryConnectionString")));

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("CacheConnectionString");
            options.InstanceName = "";
        });

        builder.Services.Add(ServiceDescriptor.Transient<IDistributedCache, RedisCache>());

        builder.Services.AddTransient<ICacheProvider, CacheProvider>();

        builder.Services.AddHangfireServer();
        builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("QueryConnectionString")));

        builder.Services.AddHostedService<RecurringJobService>();



        builder.Services.AddCors(setupAction =>
        setupAction.AddPolicy("MyPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        ));

        return builder;
    }

    public static WebApplication ConfigurePipelines(this WebApplication app)
    {
        var env = app.Services.GetRequiredService<IWebHostEnvironment>();
        var liparOptions = app.Services.GetRequiredService<LiparOptions>();

        app.UseCors("MyPolicy");

        app.AddLiparConfiguration(env, liparOptions);
        app.UseHangfireDashboard("/hangfire");

        return app;
    }
}
