using Library.Core.Domain.Products.Events;
using Lipar.Core.Domain.Entities;

namespace Library.Core.Domain.Products.Entities
{
    public class Product : AggregateRoot, IAuditable
    {
        public string Name { get; private set; }
        public string Barcode { get; private set; }

        private Product() { }

        public Product(EntityId id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;

            Apply(new ProductCreated(Id.ToString(), Barcode, Name));
        }
    }
}
