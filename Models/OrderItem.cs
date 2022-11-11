namespace OrderProcessingMVC.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public long OrderId { get; set; }

        public Order? Order { get; set; }
    }
}
