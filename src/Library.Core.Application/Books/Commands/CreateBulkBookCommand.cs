using Library.Core.Domain.Books.Entities;
using Library.Core.Domain.Books.Models;
using Library.Core.Domain.Books.Repositories;
using Library.Infrustracture.Tools.Cache.Redis;
using Lipar.Core.Contract.Common;
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
            private readonly IBookCommandRepository _repository;
            private readonly ICacheProvider _cache;

            public CreateBulkBookCommandHandler(IBookCommandRepository repository, ICacheProvider cache)
            {
                _repository = repository;
                _cache = cache;
            }

            public async Task Handle(CreateBulkBookCommand request, CancellationToken cancellationToken = default)
            {
                var books = new List<BookDto>();
                books.AddRange(await _cache.Get<List<BookDto>>(request.Key));

                foreach (var book in books)
                {
                    if (_repository.Exists(m => m.Name.Equals(book.Name) && m.Barcode.Equals(book.Barcode)))
                        continue;

                    await _repository.InsertAsync(new Book(Guid.NewGuid(), book.Name, book.Barcode));
                    await _repository.CommitAsync();
                }

                await _cache.Remove(request.Key);
            }
        }
    }
}
