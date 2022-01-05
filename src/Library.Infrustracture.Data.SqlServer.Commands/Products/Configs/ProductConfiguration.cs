﻿using Library.Core.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrustracture.Data.SqlServer.Commands.Products.Configs
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(m => m.Name).HasMaxLength(50);
            builder.Property(m => m.Barcode).HasMaxLength(10);
        }
    }
}
