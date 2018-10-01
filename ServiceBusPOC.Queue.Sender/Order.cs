using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusPOC.Queue.Sender
{
    public class Order
    {
        public Order (OrderType type)
        {
            this.Items = new List<Item>();
            this.OrderId = Guid.NewGuid();
            this.Type = type;
        }

        public Guid OrderId { get; }
        public OrderType Type { get; }

        public List<Item> Items { get; }

        public void AddItem (Item item) 
            => this.Items.Add(item);
    }
}
