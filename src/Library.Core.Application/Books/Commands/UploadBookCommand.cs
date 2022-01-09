using ExcelDataReader;
using FluentValidation;
using Library.Core.Domain.Books.Events;
using Library.Core.Domain.Books.Models;
using Library.Core.Domain.Books.Repositories;
using Lipar.Core.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Books.Commands
{
    public class UploadBookCommand : IRequest
    {
        public IFormFile File { get; set; }

        public class UploadBookCommandHandler : IRequestHandler<UploadBookCommand>
        {
            private readonly IBookCommandRepository repository;

            public UploadBookCommandHandler(IBookCommandRepository repository)
            {
                this.repository = repository;
            }

            public async Task Handle(UploadBookCommand request, CancellationToken cancellationToken = default)
            {

                //if (!Path.GetExtension(request.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                //    return BadRequest("File extension is not supported");

                List<BookCreated> books = new List<BookCreated>();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = new MemoryStream())
                {
                    request.File.CopyTo(stream);
                    stream.Position = 0;
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read()) //Each row of the file
                        {
                            books.Add(new BookCreated(Guid.NewGuid().ToString(), reader.GetValue(0).ToString(), reader.GetValue(1).ToString())) ;
                        }
                    }

                    
                }

                //var entity = new Book(Guid.NewGuid(), request.Name, request.Barcode);

                //await repository.InsertAsync(entity);
                //await repository.CommitAsync();
            }
        }

        public class UploadBookValidator : AbstractValidator<UploadBookCommand>
        {
            public UploadBookValidator()
            {
                this.CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.File)
                    .NotNull().WithMessage("File is empty")
                    .NotEmpty().WithMessage("File is empty")
                    .Must(m => Path.GetExtension(m.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)).WithMessage("File extension is not supported");
            }
        }
    }
}
