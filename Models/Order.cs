namespace OrderProcessingMVC.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public long ProviderId { get; set; }

        public OrderItem? OrderItem { get; set; }
        public Provider? Provider { get; set; }
    }
}
