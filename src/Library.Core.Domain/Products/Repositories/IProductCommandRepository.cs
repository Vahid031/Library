using Library.Core.Domain.Products.Entities;
using Lipar.Core.Domain.Data;

namespace Library.Core.Domain.Products.Repositories
{
    public interface IProductCommandRepository : ICommandRepository<Product>
    {
    }
}
