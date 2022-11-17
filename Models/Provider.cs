namespace OrderProcessingMVC.Models
{
    public class Provider
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
