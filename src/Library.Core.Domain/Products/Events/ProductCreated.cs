using Lipar.Core.Domain.Events;

namespace Library.Core.Domain.Books.Events
{
    public class BookCreated : IEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Barcode { get; }

        private BookCreated() { }

        public BookCreated(string id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;
        }
    }
}
