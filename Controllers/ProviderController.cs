using System;
using System.Collections.Generic;
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
    public class ProviderController : Controller
    {
        private readonly ProvidersRepository _providersRepository;

        public ProviderController(OrderContext context)
        {
            _providersRepository = new ProvidersRepository(context);
        }

        // GET: Provider
        public async Task<IActionResult> Index(string? sortBy = null,
            bool descending = false, List<string>? names = null)
        {
            try
            {
                var providers = await _providersRepository.GetProviders(sortBy, descending, names);
                return View(providers.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Sortby(string? sortBy = null,
            bool descending = false, List<string>? names = null)
        {
            var providers = await _providersRepository.GetProviders(sortBy, descending, names);
            return PartialView("Index", providers.ToList());
        }

        // GET: Provider/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                var provider = await _providersRepository.GetProvider(id);
                return View(provider);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
