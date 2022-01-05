using Lipar.Infrastructure.Data.SqlServer.Queries;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrustracture.Data.SqlServer.Queries.Common
{
    public class LibraryQueryDbContext : BaseQueryDbContext
    {
        public LibraryQueryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
