using System;
using System.Collections.Generic;

namespace PostFunction
{
    public class IntegrationMessage
    {
        public string SessionId { get; set; }
        public Content Content { get; set; }
        public string FunctionName { get; set; }


    }
    public class Content
    {

        public string BatchId { get; set; }
        public string Company { get; set; }
        public DateTime ReceivedDate { get; set; }

        public List<MessageItem> MessageItems { get; set; }

    }
    public class MessageItem
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