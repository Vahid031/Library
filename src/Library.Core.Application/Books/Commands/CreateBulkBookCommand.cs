using Library.Core.Domain.Books.Entities;
using Library.Core.Domain.Books.Models;
using Library.Core.Domain.Books.Repositories;
using Library.Infrustracture.Tools.Cache.Redis;
using Lipar.Core.Application.Common;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Books.Commands
{
    public class CreateBulkBookCommand : IRequest
    {
        public string Key { get; set; }

        public class CreateBulkBookCommandHandler : IRequestHandler<CreateBulkBookCommand>
        {
            private readonly IBookCommandRepository repository;
            private readonly ICacheProvider cache;

            public CreateBulkBookCommandHandler(IBookCommandRepository repository, ICacheProvider cache)
            {
                this.repository = repository;
                this.cache = cache;
            }

            public async Task Handle(CreateBulkBookCommand request, CancellationToken cancellationToken = default)
            {
                var books = new List<BookDto>();
                books.AddRange(await cache.Get<List<BookDto>>(request.Key));

                foreach (var book in books)
                {
                    if (repository.Exists(m => m.Name.Equals(book.Name) && m.Barcode.Equals(book.Barcode)))
                        continue;

                    await repository.InsertAsync(new Book(Guid.NewGuid(), book.Name, book.Barcode));
                    await repository.CommitAsync();
                }

                await cache.Remove(request.Key);
            }
        }
    }
}
