using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Models;
using OrderProcessingMVC.Repositories;

namespace OrderProcessingMVC.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly OrderItemRepository _orderItemRepository;
        private readonly OrdersRepository _ordersReprository;

        public OrderItemController(OrderItemRepository orderItemRepository, OrdersRepository ordersRepository)
        {
            _orderItemRepository = orderItemRepository;
            _ordersReprository = ordersRepository;
        }

        // GET: OrderItem
        public async Task<IActionResult> Index(string? sortBy = null, bool descending = false)
        {
            try
            {
                var orderItems = await _orderItemRepository.GetOrderItemsAsync(sortBy, descending);
                return View(orderItems.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Sortby(string? sortBy = null, bool descending = false)
        {
            try
            {
                var orderItems = await _orderItemRepository.GetOrderItemsAsync(sortBy, descending);
                return PartialView("Index", orderItems.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: OrderItem/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                var order = await _orderItemRepository.GetOrderItemAsync(id);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: OrderItem/Create
        public async Task<IActionResult> CreateAsync()
        {
            try
            {
                ViewData["OrderId"] = new SelectList(await _ordersReprository.GetOrdersAsync(), nameof(Order.Id), nameof(Order.Number));
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: OrderItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(nameof(Order.Id), nameof(OrderItem.Name), nameof(OrderItem.Quantity), nameof(OrderItem.Unit), nameof(OrderItem.OrderId))] OrderItem orderItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _orderItemRepository.AddOrderAsync(orderItem);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["OrderId"] = new SelectList(await _ordersReprository.GetOrdersAsync(), nameof(Order.Id), nameof(Order.Number), orderItem.OrderId);
                return View(orderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: OrderItem/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetOrderItemAsync(id);
                ViewData["OrderId"] = new SelectList(await _ordersReprository.GetOrdersAsync(), nameof(Order.Id), nameof(Order.Number), orderItem.OrderId);
                return View(orderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: OrderItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind(nameof(Order.Id), nameof(OrderItem.Name), nameof(OrderItem.Quantity), nameof(OrderItem.Unit), nameof(OrderItem.OrderId))] OrderItem orderItem)
        {
            try
            {
                if (id != orderItem.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _orderItemRepository.EditOrderAsync(orderItem);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["OrderId"] = new SelectList(await _ordersReprository.GetOrdersAsync(), nameof(Order.Id), nameof(Order.Number), orderItem.OrderId);
                return View(orderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: OrderItem/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            try
            {
                var order = await _orderItemRepository.GetOrderItemAsync(id);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                await _orderItemRepository.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
