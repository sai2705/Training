using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostFunction
{
    public class Shipment
    {
        public string BatchId { get; set; }
        public string Company { get; set; }
        public DateTime ReceivedDate { get; set; }
        public List<Item> Items { get; set; }

    }
    public class Item
    {
        public string ItemNumber { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
    }
}
