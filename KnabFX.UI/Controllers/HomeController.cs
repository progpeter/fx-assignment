using KnabFX.Application.Providers;
using KnabFX.Application.Services.Cryptocurrency;
using KnabFX.Application.Services.Currency;
using KnabFX.Domain.Entities.Cryptocurrency;
using KnabFX.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KnabFX.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICryptoRatesProvider _cryptoRatesProvider;

        public HomeController(ILogger<HomeController> logger, ICryptoRatesProvider cryptoRatesProvider)
        {
            _logger = logger;
            _cryptoRatesProvider = cryptoRatesProvider;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Rates()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rates(string symbol)
        {
            if (string.IsNullOrEmpty(symbol)) return RedirectToAction("Index");

            List<CryptoRate> rates = await _cryptoRatesProvider.GetCryptoRates(symbol.ToUpper());

            if (rates != null && rates.Any())
            {
                ViewBag.Rates = rates;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}