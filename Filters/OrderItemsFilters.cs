using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Filters
{
    public static class OrderItemsFilters
    {
        private const string Name = "name";
        private const string Quantity = "quantity";
        private const string Unit = "unit";
        private const string Order = "order";

        public static IEnumerable<OrderItem> SortOrderBy(IEnumerable<OrderItem> orderItems, string sortBy,
            bool descending = false)
        {
            if (descending)
            {
                switch (sortBy.ToLower())
                {
                    case Name:
                        orderItems = orderItems.OrderByDescending(o => o.Name);
                        break;
                    case Quantity:
                        orderItems = orderItems.OrderByDescending(o => o.Quantity);
                        break;
                    case Unit:
                        orderItems = orderItems.OrderByDescending(o => o.Unit);
                        break;
                    case Order:
                        orderItems = orderItems.OrderByDescending(o => o.Order);
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
                        orderItems = orderItems.OrderBy(o => o.Name);
                        break;
                    case Quantity:
                        orderItems = orderItems.OrderBy(o => o.Quantity);
                        break;
                    case Unit:
                        orderItems = orderItems.OrderBy(o => o.Unit);
                        break;
                    case Order:
                        orderItems = orderItems.OrderByDescending(o => o.Order);
                        break;
                    default:
                        throw new Exception("Incorrect Sort Filters");
                }
            }
            return orderItems;
        }

        public static IEnumerable<OrderItem> FilterByName(IEnumerable<OrderItem> orderItems,
            IEnumerable<string> names)
        {
            orderItems = orderItems.Where(o => names.Contains(o.Name));
            return orderItems;
        }

        public static IEnumerable<OrderItem> FilterByUnit(IEnumerable<OrderItem> orderItems,
            IEnumerable<string> units)
        {
            orderItems = orderItems.Where(o => units.Contains(o.Unit));
            return orderItems;
        }
    }
}
