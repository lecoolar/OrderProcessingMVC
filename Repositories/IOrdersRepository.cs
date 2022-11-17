using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public interface IOrdersRepository
    {
        public Task<IEnumerable<Order>> GetOrdersAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNumbers = null,
            DateTime? filterStartDate = null,
            DateTime? filterEndDate = null,
            IEnumerable<long> providerIds = null);
        public Task<Order> GetOrderAsync(long? id);
        public Task AddOrderAsync(Order order);
        public Task EditOrderAsync(Order order);
        public Task DeleteOrderAsync(long id);
    }
}
