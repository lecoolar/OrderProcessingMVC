using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet]
        public async Task<IActionResult> Index(string sortBy = null, bool descending = false,
            IEnumerable<string> numbers = null,
            DateTime? filterStartDate = null,
            DateTime? filterEndDate = null,
            IEnumerable<long> providerIds = null)
        {
            if (numbers != null)
            {
                numbers = numbers.Select(HttpUtility.UrlDecode);
            }
            try
            {
                var orders = await _ordersReprository.GetOrdersAsync(sortBy, descending, numbers,
                    filterStartDate, filterEndDate, providerIds);
                ViewBag.FiltersBy = nameof(Order);

                var filtersNumbers = await _ordersReprository.GetOrdersAsync();
                var filtersProviders = await _providersRepository.GetProvidersAsync();

                ViewBag.Numbers = new MultiSelectList(filtersNumbers.Distinct(),
                nameof(Order.Number), nameof(Order.Number), numbers);

                ViewBag.Providers = new MultiSelectList(filtersProviders.Distinct(),
                nameof(Provider.Id), nameof(Provider.Name), providerIds);

                ViewBag.StartDate = filterStartDate == null ? String.Empty : filterStartDate.Value.ToString("yyyy-MM-ddTHH:mm");
                ViewBag.EndDate = filterEndDate == null ? String.Empty : filterEndDate.Value.ToString("yyyy-MM-ddTHH:mm");
                return View(orders.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Sortby(string sortBy = null, bool descending = false,
            IEnumerable<string> filterNumbers = null,
            DateTime? filterStartDate = null,
            DateTime? filterEndDate = null,
            IEnumerable<long> providerIds = null)
        {
            try
            {
                var orders = await _ordersReprository.GetOrdersAsync(sortBy, descending, filterNumbers,
                    filterStartDate, filterEndDate, providerIds);
                return PartialView("Index", orders.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Order/Details/5
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
