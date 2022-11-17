using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public class OrderItemRepository
    {
        private readonly DateBaseOrderContext _context;

        public OrderItemRepository(DateBaseOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNames = null,
            IEnumerable<string> units = null)
        {
            IEnumerable<OrderItem> orderItems = await _context.OrderItems.Include(o => o.Order).ToArrayAsync();
            if (sortBy != null)
            {
                orderItems = OrderItemsFilters.SortOrderBy(orderItems, sortBy, descending);
            }
            if (filterNames != null && filterNames.Count() != 0)
            {
                orderItems = OrderItemsFilters.FilterByName(orderItems, filterNames);
            }
            if (units != null && units.Count() != 0)
            {
                orderItems = OrderItemsFilters.FilterByUnit(orderItems, units);
            }
            return orderItems;
        }

        public async Task<OrderItem> GetOrderItemAsync(long? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                throw new Exception("NotFound");
            }

            var orderItems = await _context.OrderItems
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItems == null)
            {
                throw new Exception("NotFound");
            }
            return orderItems;
        }

        public async Task AddOrderAsync(OrderItem orderItems)
        {
            var order = await _context.Orders.Include(o => o.OrderItem)
                .FirstOrDefaultAsync(p => p.Id == orderItems.OrderId);
            if (order != null)
            {
                if (order.Number == orderItems.Name)
                {
                    throw new Exception("OrderItem name cannot be equal Order number");
                }
                if (order.OrderItem == null)
                {
                    order.OrderItem = new List<OrderItem>() { orderItems };
                }
                else
                {
                    order.OrderItem.Add(orderItems);
                }
                orderItems.Order = order;
                _context.Add(orderItems);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("NotFound Order");
            }
        }

        public async Task EditOrderAsync(OrderItem orderItem)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.Provider)
                    .FirstOrDefaultAsync(o => o.Id == orderItem.OrderId);
                if (order == null)
                {
                    throw new Exception("NotFound OrderItem");
                }
                orderItem.OrderId = order.Id;
                orderItem.Order = order;

                _context.Update(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderItem.Id))
                {
                    throw new Exception("NotFound");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteOrderAsync(long id)
        {
            if (_context.Orders == null)
            {
                throw new Exception("Entity set 'OrderContext.OrderItems'  is null.");
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
