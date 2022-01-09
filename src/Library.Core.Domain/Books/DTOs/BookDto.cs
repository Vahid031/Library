using Lipar.Core.Domain.Events;

namespace Library.Core.Domain.Books.Models
{
    public class BookDto : IEvent
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
