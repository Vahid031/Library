using Library.Core.Domain.Books.Events;
using Lipar.Core.Domain.Entities;

namespace Library.Core.Domain.Books.Entities
{
    public class Book : AggregateRoot, IAuditable
    {
        public string Name { get; private set; }
        public string Barcode { get; private set; }

        private Book() { }

        public Book(EntityId id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;

            Apply(new BookCreated(Id.ToString(), Barcode, Name));
        }
    }
}
