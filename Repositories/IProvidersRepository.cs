using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public interface IProvidersRepository
    {
        public Task<IEnumerable<Provider>> GetProvidersAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNames = null);
        public Task<Provider> GetProviderAsync(long? id);
    }
}
