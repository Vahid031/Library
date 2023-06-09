using Hangfire;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using Lipar.Core.Contract.Common;

namespace Library.Presentation.Api
{
    internal class RecurringJobService : BackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobs;
        private readonly IMediator _mediator;
        private readonly ILogger<RecurringJobScheduler> _logger;

        public RecurringJobService(
            IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobs,
            IMediator mediator,
            ILogger<RecurringJobScheduler> logger)
        {
            _backgroundJobs = backgroundJobs;
            _recurringJobs = recurringJobs;
            _mediator = mediator;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //_recurringJobs.AddOrUpdate("RecurringJobRepairRequest", () => _commandDispatcher.Send(new RecurringJobRepairRequestCommand()), "* 1 * * * *");

            //_recurringJobs.AddOrUpdate("WarehouseInventoryFromERP", () => _commandDispatcher.Send(new CreateWarehouseInventoryCommand()), "* 1 * * * *");

            //_backgroundJobs.Schedule(() => Console.WriteLine("Hello Hangfire job!"), DateTime.Now.AddSeconds(5));
            //_recurringJobs.AddOrUpdate("seconds", () => Console.WriteLine("Hello, seconds!"), "*/15 * * * * *");
            //_recurringJobs.AddOrUpdate("minutely", () => Console.WriteLine("Hello, world!"), Cron.Minutely);
            //_recurringJobs.AddOrUpdate("hourly", () => Console.WriteLine("Hello"), "25 15 * * *");
            //_recurringJobs.AddOrUpdate("neverfires", () => Console.WriteLine("Can only be triggered"), "0 0 31 2 *");

            //_recurringJobs.AddOrUpdate("Hawaiian", () => Console.WriteLine("Hawaiian"), "15 08 * * *", TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"));
            //_recurringJobs.AddOrUpdate("UTC", () => Console.WriteLine("UTC"), "15 18 * * *");
            //_recurringJobs.AddOrUpdate("Russian", () => Console.WriteLine("Russian"), "15 21 * * *", TimeZoneInfo.Local);


            return Task.CompletedTask;
        }
    }
}
