using Library.Core.Domain.Books.Entities;
using Lipar.Infrastructure.Data.SqlServer.Commands;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrustracture.Data.SqlServer.Commands.Common
{
    public class LibraryCommandDbContext : BaseCommandDbContext
    {
        public LibraryCommandDbContext(DbContextOptions<LibraryCommandDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Library;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        DbSet<Book> Books { get; set; }
    }
}