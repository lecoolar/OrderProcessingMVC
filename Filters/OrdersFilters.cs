using Microsoft.CodeAnalysis.CSharp.Syntax;
using OrderProcessingMVC.Models;
using System.Data;
using System.Linq;

namespace OrderProcessingMVC.Filters
{
    public static class OrdersFilters
    {
        private const string Number = "number";
        private const string Date = "startdate";
        private const string Provider = "provider";
        private const int AddMonths = -1;

        public static IEnumerable<Order> SortOrderBy(IEnumerable<Order> orders, string sortBy,
            bool descending = false)
        {
            if (descending)
            {
                switch (sortBy.ToLower())
                {
                    case Number:
                        orders.OrderByDescending(o => o.Number);
                        break;
                    case Date:
                        orders.OrderByDescending(o => o.Date);
                        break;
                    case Provider:
                        orders.OrderByDescending(o => o.Provider);
                        break;
                    default:
                        throw new Exception("Incorrect Sort Filters");
                }
            }
            else
            {
                switch (sortBy.ToLower())
                {
                    case Number:
                        orders.OrderBy(o => o.Number);
                        break;
                    case Date:
                        orders.OrderBy(o => o.Date);
                        break;
                    case Provider:
                        orders.OrderBy(o => o.Provider);
                        break;
                    default:
                        throw new Exception("Incorrect Sort Filters");
                }
            }
            return orders;
        }

        public static IEnumerable<Order> FilterByNumber(IEnumerable<Order> orders,
            IEnumerable<string> numbers)
        {
            orders = orders.Where(o => numbers.Contains(o.Number));
            return orders;
        }

        public static IEnumerable<Order> FilterByDate(IEnumerable<Order> orders,
            string filterSartdate, string filterEndDate)
        {
            DateTime startDate;
            DateTime endDate;

            if (filterSartdate == null && filterEndDate != null)
            {
                startDate = DateTime.UtcNow;
            }

            if (filterSartdate != null && filterEndDate == null)
            {
                endDate = DateTime.UtcNow.AddMonths(AddMonths);
            }

            else if (DateTime.TryParse(filterSartdate, out startDate)
                && DateTime.TryParse(filterEndDate, out endDate))
            {
                if (startDate > endDate)
                {
                    throw new Exception("Start date cannot be more that end date");
                }
                else
                {
                    orders = orders.Where(o => o.Date >= startDate && o.Date <= endDate);
                }
            }

            else
            {
                throw new Exception("Incorrect dates");
            }

            return orders;
        }

        public static IEnumerable<Order> FilterByProvider(IEnumerable<Order> orders,
            IEnumerable<Provider> providers)
        {
            orders = orders.Where(o => providers.Contains(o.Provider));
            return orders;
        }
    }
}

