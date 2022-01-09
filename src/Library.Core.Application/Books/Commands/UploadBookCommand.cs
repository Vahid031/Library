using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using Library.Core.Domain.Books.Entities;
using Library.Core.Domain.Books.Events;
using Library.Core.Domain.Books.Models;
using Library.Core.Domain.Books.Repositories;
using Library.Infrustracture.Tools.Cache.Redis;
using Lipar.Core.Application.Common;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Application.Books.Commands
{
    public class UploadBookCommand : IRequest
    {
        public string Key { get; set; }
        public IFormFile File { get; set; }

        public class UploadBookCommandHandler : IRequestHandler<UploadBookCommand>
        {
            private readonly IBookCommandRepository repository;
            private readonly ICacheProvider cache;
            private readonly IJson json;

            public UploadBookCommandHandler(IBookCommandRepository repository, ICacheProvider cache, IJson json)
            {
                this.repository = repository;
                this.cache = cache;
                this.json = json;
            }

            public async Task Handle(UploadBookCommand request, CancellationToken cancellationToken = default)
            {

                List<BookDto> books = new List<BookDto>();

                using (var stream = new MemoryStream())
                {
                    request.File.CopyTo(stream);
                    stream.Position = 0;

                    var cacheOptions = new DistributedCacheEntryOptions();

                    using (var document = SpreadsheetDocument.Open(stream, true))
                    {

                        foreach (var wp in document.WorkbookPart.WorksheetParts)
                        {
                            Worksheet worksheet = wp.Worksheet;

                            var rows = worksheet.GetFirstChild<SheetData>().Elements<Row>();

                            //Dictionary<string, string> dic = new Dictionary<string, string>();

                            //foreach (var cell in worksheet.GetFirstChild<SheetData>().GetFirstChild<Row>())
                            //{
                            //    dic.Add(cell.CellValue.Text);
                            //}

                            worksheet.GetFirstChild<SheetData>().RemoveChild(worksheet.GetFirstChild<SheetData>().GetFirstChild<Row>());

                            var result = Parallel.ForEach(rows, row =>
                            {
                                var cells = row.Elements<Cell>().ToList();

                                books.Add(new BookDto { Name = cells[0].CellValue.Text, Barcode = cells[1].CellValue.Text });

                            });

                            if (result.IsCompleted)
                            {
                                await cache.Set(request.Key, books, cacheOptions);
                            }
                        }

                        document.Close();
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
