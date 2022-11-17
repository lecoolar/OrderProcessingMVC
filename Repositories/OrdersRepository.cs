using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;
using System.Collections.Immutable;

namespace OrderProcessingMVC.Repositories
{
    public class OrdersRepository: IOrdersRepository
    {
        private readonly DateBaseOrderContext _context;

        public OrdersRepository(DateBaseOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNumbers = null,
            DateTime? filterStartDate = null,
            DateTime? filterEndDate = null,
            IEnumerable<long> providerIds = null)
        {
            IEnumerable<Order> orders = await _context.Orders.Include(o => o.Provider).ToArrayAsync();
            if (sortBy != null)
            {
                orders = OrdersFilters.SortOrderBy(orders, sortBy, descending);
            }
            if (filterNumbers != null && filterNumbers.Count() != 0)
            {
                orders = OrdersFilters.FilterByNumber(orders, filterNumbers);
            }
            if (filterStartDate != null || filterEndDate != null)
            {
                orders = OrdersFilters.FilterByDate(orders, filterStartDate, filterEndDate);
            }
            if (providerIds != null && providerIds.Count() != 0)
            {
                orders = OrdersFilters.FilterByProvider(orders, providerIds);
            }
            return orders;
        }

        public async Task<Order> GetOrderAsync(long? id)
        {
            if (id == null || _context.Orders == null)
            {
                throw new Exception("NotFound");
            }

            var order = await _context.Orders
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                throw new Exception("NotFound");
            }
            return order;
        }

        public async Task AddOrderAsync(Order order)
        {
            var provider = await _context.Providers.Include(p => p.Orders).FirstOrDefaultAsync(p => p.Id == order.ProviderId);
            if (provider != null)
            {
                if (provider.Orders == null)
                {
                    provider.Orders = new List<Order>() { order };
                }
                else
                {
                    if (provider.Orders.FirstOrDefault(o => o.Number == order.Number) != null)
                    {
                        throw new Exception("Order number must be unique");
                    }
                    else
                    {
                        provider.Orders.Add(order);
                    }
                }
                order.Provider = provider;
                _context.Add(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("NotFound Provider");
            }
        }

        public async Task EditOrderAsync(Order order)
        {
            try
            {
                var provider = await _context.Providers.Include(p => p.Orders)
                    .FirstOrDefaultAsync(p => p.Id == order.ProviderId);
                if (provider != null)
                {
                    if (provider.Orders == null)
                    {
                        throw new Exception("NotFound Orders");
                    }
                    if (provider.Orders.FirstOrDefault(o => o.Number == order.Number) != null)
                    {
                        throw new Exception("Order number must be unique");
                    }
                    else
                    {
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    throw new Exception("NotFound Provider");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
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
                throw new Exception("Entity set 'OrderContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            var orderItem = await _context.OrderItems.Where(o=>o.OrderId==id).ToArrayAsync();
            if (orderItem != null)
            {
                _context.OrderItems.RemoveRange(orderItem);
            }
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
