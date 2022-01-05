using Library.Core.Domain.Products.Repositories;
using Library.Infrustracture.Data.SqlServer.Queries.Common;
using Lipar.Infrastructure.Data.SqlServer.Queries;

namespace Library.Infrustracture.Data.SqlServer.Queries.Products
{
    public class ProductQueryRepository : BaseQueryRepository<LibraryQueryDbContext>,
        IProductQueryRepository
    {
        public ProductQueryRepository(LibraryQueryDbContext db) : base(db)
        {
        }
    }
}
