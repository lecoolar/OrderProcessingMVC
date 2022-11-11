using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public class ProvidersRepository
    {
        private readonly OrderContext _context;

        public ProvidersRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Provider>> GetProviders(string? sortBy = null, bool descending = false,
            List<string>? filterName = null)
        {
            IEnumerable<Provider> provider = await _context.Providers.Include(o => o.Orders).ToArrayAsync();
            if (sortBy != null)
            {
                provider = ProvidersFilters.SortOrderBy(provider, sortBy, descending);
            }
            if (filterName != null)
            {
                provider = ProvidersFilters.FilterByName(provider, filterName);
            }
            return provider;
        }
    }
}
