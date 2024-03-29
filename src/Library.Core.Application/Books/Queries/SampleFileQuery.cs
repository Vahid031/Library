﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Library.Core.Domain.Books.Models;
using Lipar.Core.Contract.Common;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Contract.Books.Queries
{
    public class SampleFileQuery : IRequest<Stream>
    {
        public uint RowCount { get; set; } = 500000;

        public class SampleFileQueryHandler : IRequestHandler<SampleFileQuery, Stream>
        {
            Task<Stream> IRequestHandler<SampleFileQuery, Stream>.Handle(SampleFileQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(WriteExcelFile(request.RowCount));
            }
        }

        static Stream WriteExcelFile(uint rowCount)
        {
            Stream stream = new MemoryStream();
            
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                BookDto book = new BookDto();
                Row row = new Row();

                foreach (var prop in typeof(BookDto).GetProperties())
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(prop.Name);
                    row.AppendChild(cell);
                }

                sheetData.AppendChild(row);

                for (int i = 0; i < rowCount; i++)
                {
                    book.Barcode = string.Format("{0:10000000}", i + 1);
                    book.Name = Guid.NewGuid().ToString().Substring(0, 7);

                    row = new Row();

                    foreach (var prop in typeof(BookDto).GetProperties())
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(prop.GetValue(book).ToString());
                        row.AppendChild(cell);
                    }

                    sheetData.AppendChild(row);
                }

                document.Save();
                document.Close();
            }

            stream.Position = 0;
            return stream;
        }
    }
}
