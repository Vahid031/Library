using Library.Core.Domain.Books.Entities;
using Lipar.Core.Domain.Data;

namespace Library.Core.Domain.Books.Repositories
{
    public interface IBookCommandRepository : ICommandRepository<Book>
    {
    }
}
