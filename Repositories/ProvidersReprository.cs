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
            if (!context.Providers.Any())
            {
                context.Providers.Add(new Provider() { Name = "Магнит" });
                context.Providers.Add(new Provider() { Name = "Пятерочка" });
                context.Providers.Add(new Provider() { Name = "Красное белое" });
                context.Providers.Add(new Provider() { Name = "Гуливер" });
                context.Providers.Add(new Provider() { Name = "Лента" });
                context.Providers.Add(new Provider() { Name = "Карусель" });
                context.SaveChanges();
            }
            _context = context;
        }

        public async Task<IEnumerable<Provider>> GetProviders(string? sortBy = null, bool descending = false,
            List<string>? filterName = null)
        {
            IEnumerable<Provider> providers = await _context.Providers.Include(p => p.Orders).ToArrayAsync();
            if (sortBy != null)
            {
                providers = ProvidersFilters.SortOrderBy(providers, sortBy, descending);
            }
            if (filterName != null && !providers.Any())
            {
                providers = ProvidersFilters.FilterByName(providers, filterName);
            }
            return providers;
        }

        public async Task<Provider> GetProvider(long? id)
        {
            if (id == null || _context.Providers == null)
            {
                throw new Exception("NotFound");
            }

            var provider = await _context.Providers
                .Include(o => o.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                throw new Exception("NotFound");
            }
            return provider;
        }
    }
}
