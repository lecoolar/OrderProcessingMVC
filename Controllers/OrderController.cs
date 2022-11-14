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

        public OrderController(OrdersRepository ordersRepository, ProvidersRepository providersRepository)
        {
            _providersRepository = providersRepository;
            _ordersReprository = ordersRepository;
        }

        // GET: Order
        public async Task<IActionResult> Index(string? sortBy = null, bool descending = false)
        {
            try
            {
                var orders = await _ordersReprository.GetOrdersAsync(sortBy, descending);
                return View(orders.ToList());
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
                var orders = await _ordersReprository.GetOrdersAsync(sortBy, descending);
                return PartialView("Index", orders.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                var order = await _ordersReprository.GetOrderAsync(id);
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
            try
            {
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProvidersAsync(), nameof(Provider.Id), nameof(Provider.Name));
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(nameof(Order.Id), nameof(Order.Number), nameof(Order.Date), nameof(Order.ProviderId))] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _ordersReprository.AddOrderAsync(order);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProvidersAsync(), nameof(Provider.Id), nameof(Provider.Name), order.ProviderId);
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
                var order = await _ordersReprository.GetOrderAsync(id);
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProvidersAsync(), nameof(Provider.Id), nameof(Provider.Name), order.ProviderId);
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
        public async Task<IActionResult> Edit(long id, [Bind(nameof(Order.Id), nameof(Order.Number), nameof(Order.Date), nameof(Order.ProviderId))] Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _ordersReprository.EditOrderAsync(order);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProviderId"] = new SelectList(await _providersRepository.GetProvidersAsync(), nameof(Provider.Id), nameof(Provider.Name), order.ProviderId);
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
                var order = await _ordersReprository.GetOrderAsync(id);
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
                await _ordersReprository.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
