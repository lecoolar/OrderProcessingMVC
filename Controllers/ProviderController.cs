using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Models;
using OrderProcessingMVC.Repositories;

namespace OrderProcessingMVC.Controllers
{
    public class ProviderController : Controller
    {
        private readonly ProvidersRepository _providersRepository;

        public ProviderController(ProvidersRepository providersRepository)
        {
            _providersRepository = providersRepository;
        }

        // GET: Provider
        [HttpGet]
        public async Task<IActionResult> Index(string? sortBy = null,
            bool descending = false, IEnumerable<string>? filterNames = null)
        {
            try
            {
                var providers = await _providersRepository.GetProvidersAsync(sortBy, descending, filterNames);

                ViewBag.FiltersBy = nameof(Provider);

                var names = await _providersRepository.GetProvidersAsync();

                ViewBag.FilterNames = new MultiSelectList(names.Distinct(),
                    nameof(Provider.Name), nameof(Provider.Name), filterNames);
                return View(providers.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Sortby(string? sortBy = null,
            bool descending = false, IEnumerable<string>? filterNames = null)
        {
            if (filterNames != null)
            {
                filterNames = filterNames.Select(HttpUtility.UrlDecode);
            }
            try
            {
                var providers = await _providersRepository.GetProvidersAsync(sortBy, descending, filterNames);
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
