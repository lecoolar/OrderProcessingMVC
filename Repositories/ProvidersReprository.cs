using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public class ProvidersRepository
    {
        private readonly DateBaseOrderContext _context;

        public ProvidersRepository(DateBaseOrderContext context)
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

        public async Task<IEnumerable<Provider>> GetProvidersAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNames = null)
        {
            IEnumerable<Provider> providers = await _context.Providers.Include(p => p.Orders).ToArrayAsync();
            if (sortBy != null)
            {
                providers = ProvidersFilters.SortOrderBy(providers, sortBy, descending);
            }
            if (filterNames != null && filterNames.Count() != 0)
            {
                providers = ProvidersFilters.FilterByName(providers, filterNames);
            }
            return providers;
        }

        public async Task<Provider> GetProviderAsync(long? id)
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
