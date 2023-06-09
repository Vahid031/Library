using FluentValidation;
using Library.Core.Domain.Books.Entities;
using Library.Core.Domain.Books.Repositories;
using Lipar.Core.Contract.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Books.Commands
{
    public class CreateBookCommand : IRequest
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand>
        {
            private readonly IBookCommandRepository _repository;

            public CreateBookCommandHandler(IBookCommandRepository repository)
            {
                _repository = repository;
            }

            public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken = default)
            {
                var entity = new Book(Guid.NewGuid(), request.Name, request.Barcode);

                await _repository.InsertAsync(entity);
                await _repository.CommitAsync();
            }
        }

        public class CreateBookValidator : AbstractValidator<CreateBookCommand>
        {
            public CreateBookValidator()
            {
                RuleFor(m => m.Name)
                    .NotEmpty()
                    .MinimumLength(7);

                RuleFor(m => m.Barcode)
                    .Must(m => uint.TryParse(m, out uint s));
            }
        }
    }
}
