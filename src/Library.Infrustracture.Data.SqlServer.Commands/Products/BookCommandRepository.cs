using Library.Core.Domain.Books.Entities;
using Library.Core.Domain.Books.Repositories;
using Library.Infrustracture.Data.SqlServer.Commands.Common;
using Lipar.Infrastructure.Data.SqlServer.Commands;

namespace Library.Infrustracture.Data.SqlServer.Commands.Books
{
    public class BookCommandRepository : BaseCommandRepository<Book, LibraryCommandDbContext>,
        IBookCommandRepository
    {
        public BookCommandRepository(LibraryCommandDbContext db) : base(db)
        {
        }
    }
}
