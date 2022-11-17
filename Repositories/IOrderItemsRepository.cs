using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public interface IOrderItemsRepository
    {
        public Task<IEnumerable<OrderItem>> GetOrderItemsAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNames = null,
            IEnumerable<string> units = null);
        public Task<OrderItem> GetOrderItemAsync(long? id);
        public Task AddOrderAsync(OrderItem orderItems);
        public Task EditOrderAsync(OrderItem orderItem);
        public Task DeleteOrderAsync(long id);

    }
}
