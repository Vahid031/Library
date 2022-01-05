using Library.Core.Domain.Products.Entities;
using Library.Core.Domain.Products.Repositories;
using Library.Infrustracture.Data.SqlServer.Commands.Common;
using Lipar.Infrastructure.Data.SqlServer.Commands;

namespace Library.Infrustracture.Data.SqlServer.Commands.Products
{
    public class ProductCommandRepository : BaseCommandRepository<Product, LibraryCommandDbContext>,
        IProductCommandRepository
    {
        public ProductCommandRepository(LibraryCommandDbContext db) : base(db)
        {
        }
    }
}
