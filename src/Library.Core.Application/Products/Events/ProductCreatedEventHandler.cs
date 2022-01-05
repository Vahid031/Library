using Library.Core.Domain.Products.Events;
using Lipar.Core.Application.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Products.Events
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreated>
    {
        public Task Handle(ProductCreated @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
