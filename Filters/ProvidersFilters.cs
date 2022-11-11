﻿using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Filters
{
    public static class ProvidersFilters
    {
        private const string Name = "number";


        public static IEnumerable<Provider> SortOrderBy(IEnumerable<Provider> providers, string sortBy,
            bool descending = false)
        {
            if (descending)
            {
                switch (sortBy.ToLower())
                {
                    case Name:
                        providers.OrderByDescending(o => o.Name);
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
                        providers.OrderBy(o => o.Name);
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
            providers = providers.Where(o => names.Contains(o.Name));
            return providers;
        }
    }
}
