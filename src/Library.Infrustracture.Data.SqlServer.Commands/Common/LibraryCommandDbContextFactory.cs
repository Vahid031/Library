using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Infrustracture.Data.SqlServer.Commands.Common
{
    public class LibraryCommandDbContextFactory : IDesignTimeDbContextFactory<LibraryCommandDbContext>
    {
        public LibraryCommandDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LibraryCommandDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Library;Trusted_Connection=True;");
            return new LibraryCommandDbContext(builder.Options);
        }
    }
}