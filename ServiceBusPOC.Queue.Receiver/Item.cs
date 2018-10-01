namespace ServiceBusPOC.Queue.Receiver
{
    public class Item
    {
        public Item(string name, decimal unitPrice, int quantity)
        {
            this.Name = name;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }

        public string Name { get; }
        public decimal UnitPrice { get; }
        public decimal TotalPrice => this.UnitPrice * this.Quantity;
        public int Quantity { get; }
    }
}