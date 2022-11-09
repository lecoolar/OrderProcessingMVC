using System;
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
        private readonly OrdersRepository _reprository;

        public OrderController(OrdersRepository context)
        {
            _reprository = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            try
            {
                var orderContext = await _reprository.GetOrders();
                return View(orderContext.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var order = await _reprository.GetOrder(id);

            return View(order);
        }

        //// GET: Order/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ProviderId"] = new SelectList(await _reprository.GetProviders(), "Id", "Id");
            return View();
        }

        //// POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ProviderId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _reprository.AddOrder(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(await _reprository.GetProviders(), "Id", "Id", order.ProviderId);
            return View(order);
        }

        //// GET: Order/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {

            var order = await _reprository.GetOrder(id);
            ViewData["ProviderId"] = new SelectList(await _reprository.GetProviders(), "Id", "Id", order.ProviderId);
            return View(order);
        }

        //// POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Number,Date,ProviderId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _reprository.EditOrder(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(await _reprository.GetProviders(), "Id", "Id", order.ProviderId);
            return View(order);
        }

        //// GET: Order/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var order = await _reprository.GetOrder(id);
            return View(order);
        }

        //// POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            _reprository.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
