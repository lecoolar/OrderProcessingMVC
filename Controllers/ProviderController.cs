using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderProcessingMVC.Models;
using OrderProcessingMVC.Repositories;

namespace OrderProcessingMVC.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProvidersRepository _providersRepository;

        public ProviderController(IProvidersRepository providersRepository)
        {
            _providersRepository = providersRepository;
        }

        // GET: Provider
        [HttpGet]
        public async Task<IActionResult> Index(string sortBy = null,
            bool descending = false, IEnumerable<string> names = null)
        {
            try
            {
                var providers = await _providersRepository.GetProvidersAsync(sortBy, descending, names);

                ViewBag.FiltersBy = nameof(Provider);

                var filterNames = await _providersRepository.GetProvidersAsync();

                ViewBag.FilterNames = new MultiSelectList(filterNames.Distinct(),
                    nameof(Provider.Name), nameof(Provider.Name), names);
                return View(providers.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Sortby(string sortBy = null,
            bool descending = false, IEnumerable<string> names = null)
        {
            if (names != null)
            {
                names = names.Select(HttpUtility.UrlDecode);
            }
            try
            {
                var providers = await _providersRepository.GetProvidersAsync(sortBy, descending, names);
                return PartialView("Index", providers.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Provider/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                var provider = await _providersRepository.GetProviderAsync(id);
                return View(provider);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
