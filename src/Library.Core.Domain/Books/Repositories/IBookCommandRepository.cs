using Library.Core.Domain.Books.Entities;
using Lipar.Core.Contract.Data;

namespace Library.Core.Domain.Books.Repositories
{
    public interface IBookCommandRepository : ICommandRepository<Book>
    {
    }
}
