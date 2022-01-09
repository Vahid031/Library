using Library.Core.Domain.Books.Repositories;
using Library.Infrustracture.Data.SqlServer.Queries.Common;
using Lipar.Infrastructure.Data.SqlServer.Queries;

namespace Library.Infrustracture.Data.SqlServer.Queries.Books
{
    public class BookQueryRepository : BaseQueryRepository<LibraryQueryDbContext>,
        IBookQueryRepository
    {
        public BookQueryRepository(LibraryQueryDbContext db) : base(db)
        {
        }
    }
}
