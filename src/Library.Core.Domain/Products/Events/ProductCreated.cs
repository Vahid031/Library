﻿using Lipar.Core.Domain.Events;

namespace Library.Core.Domain.Products.Events
{
    public class ProductCreated : IEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Barcode { get; }

        private ProductCreated() { }

        public ProductCreated(string id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;
        }
    }
}