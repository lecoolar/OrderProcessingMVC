using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;
using System.Collections.Immutable;

namespace OrderProcessingMVC.Repositories
{
    public class OrdersRepository
    {
        private readonly OrderContext _context;

        public OrdersRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrders(string sortBy = null, bool descending = false,
            List<string> filterNumbers = null,
            string filterStartDate = null,
            string filterEndDate = null,
            List<Provider> providers = null)
        {
            IEnumerable<Order> orders = await _context.Orders.Include(o => o.Provider).ToArrayAsync();
            if (sortBy != null)
            {
                orders = OrdersFilters.SortOrderBy(orders, sortBy, descending);
            }
            if (filterNumbers != null)
            {
                orders = OrdersFilters.FilterByNumber(orders, filterNumbers);
            }
            if (filterStartDate != null || filterEndDate != null)
            {
                orders = OrdersFilters.FilterByDate(orders, filterStartDate, filterEndDate);
            }
            if (providers != null)
            {
                orders = OrdersFilters.FilterByProvider(orders, providers);
            }
            return orders;
        }

        public async Task<Order> GetOrder(long? id)
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

        public async void AddOrder(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
        }

        public async void EditOrder(Order order)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
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

        public async void DeleteOrder(long id)
        {
            if (_context.Orders == null)
            {
                throw new Exception("Entity set 'OrderContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Provider>> GetProviders()
        {
            var providers = await _context.Providers.ToArrayAsync();
            return providers;
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
