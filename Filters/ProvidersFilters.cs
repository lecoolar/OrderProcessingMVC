using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Filters
{
    public static class ProvidersFilters
    {
        private const string Name = "name";


        public static IEnumerable<Provider> SortOrderBy(IEnumerable<Provider> providers, string sortBy,
            bool descending = false)
        {
            if (descending)
            {
                switch (sortBy.ToLower())
                {
                    case Name:
                        providers = providers.OrderByDescending(o => o.Name);
                        break;
                    default:
                        throw new Exception("Incorrect Sort Filters");
                }
            }
            else
            {
                switch (sortBy.ToLower())
                {
                    case Name:
                        providers = providers.OrderBy(o => o.Name);
                        break;
                    default:
                        throw new Exception("Incorrect Sort Filters");
                }
            }
            return providers;
        }

        public static IEnumerable<Provider> FilterByName(IEnumerable<Provider> providers,
            IEnumerable<string> names)
        {
            providers = providers.Where(p => names.Contains(p.Name));
            return providers;
        }
    }
}
