namespace OrderProcessingMVC.Models
{
    public class Provider
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public virtual Order? Order {get; set; } 
    }
}
