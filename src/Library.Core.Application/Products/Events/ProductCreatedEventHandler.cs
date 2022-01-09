using Library.Core.Domain.Books.Events;
using Lipar.Core.Application.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Books.Events
{
    public class BookCreatedEventHandler : IEventHandler<BookCreated>
    {
        public Task Handle(BookCreated @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
