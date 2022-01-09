using Library.Core.Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrustracture.Data.SqlServer.Commands.Books.Configs
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(m => m.Name).HasMaxLength(50);
            builder.Property(m => m.Barcode).HasMaxLength(10);
        }
    }
}
