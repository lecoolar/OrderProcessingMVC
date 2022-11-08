namespace OrderProcessingMVC.Models
{
    public class Provider
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? OwnerId { get; set; }
        public Order? Order {get; set; } 
    }
}
