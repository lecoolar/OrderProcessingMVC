using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Models;
using OrderProcessingMVC.Repositories;

namespace OrderProcessingMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrdersRepository _ordersReprository;
        private readonly ProvidersRepository _providersRepository;

        public OrderController(OrderContext context)
        {
            _providersRepository = new ProvidersRepository(context);
            _ordersReprository = new OrdersRepository(context);

        }

        // GET: Order
        public async Task<IActionResult> Index(string? sortBy = null, bool descending = false)
        {
            try
            {
                var orderContext = await _ordersReprository.GetOrders(sortBy, descending);
                return View(orderContext.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Sortby(string? sortBy = null, bool descending = false)
        {
            var orders = await _ordersReprository.GetOrders(sortBy, descending);
            return PartialView("Index", orders.ToList());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                var order = await _ordersReprository.GetOrder(id);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //// GET: Order/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProviders(), "Id", "Name");
            return View();
        }

        //// POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ProviderId")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ordersReprository.AddOrder(order);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProviders(), "Id", "Name", order.ProviderId);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// GET: Order/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            try
            {
                var order = await _ordersReprository.GetOrder(id);
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProviders(), "Id", "Name", order.ProviderId);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Number,Date,ProviderId")] Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _ordersReprository.EditOrder(order);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProviders(), "Id", "Name", order.ProviderId);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// GET: Order/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            try
            {
                var order = await _ordersReprository.GetOrder(id);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                _ordersReprository.DeleteOrder(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
