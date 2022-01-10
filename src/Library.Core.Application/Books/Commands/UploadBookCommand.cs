using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using Library.Core.Domain.Books.Models;
using Library.Infrustracture.Tools.Cache.Redis;
using Lipar.Core.Application.Common;
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
            private readonly ICacheProvider _cache;

            public UploadBookCommandHandler(ICacheProvider cache)
            {
                _cache = cache;
            }

            public async Task Handle(UploadBookCommand request, CancellationToken cancellationToken = default)
            {
                var cacheOptions = new DistributedCacheEntryOptions();
                var books = ExelTolist(request.File);
                await _cache.Set(request.Key, books, cacheOptions);
            }
        }

        static List<BookDto> ExelTolist(IFormFile file)
        {
            List<BookDto> books = new List<BookDto>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;


                using (var document = SpreadsheetDocument.Open(stream, true))
                {
                    foreach (var wp in document.WorkbookPart.WorksheetParts)
                    {
                        Worksheet worksheet = wp.Worksheet;

                        var rows = worksheet.GetFirstChild<SheetData>().Elements<Row>();

                        worksheet.GetFirstChild<SheetData>().RemoveChild(worksheet.GetFirstChild<SheetData>().GetFirstChild<Row>());

                        foreach (var row in rows)
                        {
                            var cells = row.Elements<Cell>().ToList();

                            books.Add(new BookDto { Name = cells[0].CellValue.Text, Barcode = cells[1].CellValue.Text });
                        }
                    }
                    document.Close();
                }

                return books;
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
