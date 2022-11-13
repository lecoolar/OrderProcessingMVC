﻿using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Filters;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Repositories
{
    public class OrderItemRepository
    {
        private readonly OrderContext _context;

        public OrderItemRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItems(string? sortBy = null, bool descending = false,
            List<string>? filterNames = null,
            List<string>? units = null)
        {
            IEnumerable<OrderItem> orderItems = await _context.OrderItems.Include(o => o.Order).ToArrayAsync();
            if (sortBy != null)
            {
                orderItems = OrderItemsFilters.SortOrderBy(orderItems, sortBy, descending);
            }
            if (filterNames != null)
            {
                orderItems = OrderItemsFilters.FilterByName(orderItems, filterNames);
            }
            if (units != null)
            {
                orderItems = OrderItemsFilters.FilterByUnit(orderItems, units);
            }
            return orderItems;
        }

        public async Task<OrderItem> GetOrderItem(long? id)
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

        public async void AddOrder(OrderItem orderItems)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == orderItems.OrderId);
            if (order != null)
            {
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

        public async void EditOrder(OrderItem orderItem)
        {
            try
            {
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

        public async void DeleteOrder(long id)
        {
            if (_context.Orders == null)
            {
                throw new Exception("Entity set 'OrderContext.OrderItems'  is null.");
            }
            var order = await _context.OrderItems.FindAsync(id);
            if (order != null)
            {
                _context.OrderItems.Remove(order);
            }

            await _context.SaveChangesAsync();
        }



        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
