using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;
using EcoSystem.Models;
using EcoSystem.Services;

namespace EcoSystem.Controllers
{
    public class SurveyController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ScoreCalculator _calculator;

        public SurveyController(AppDbContext db, ScoreCalculator calculator)
        {
            _db = db;
            _calculator = calculator;
        }

        // ==================== ЭКРАН 1 ====================
        [HttpGet]
        public async Task<IActionResult> Step1()
        {
            var model = new Step1ViewModel
            {
                Cities = await _db.Cities.OrderBy(c => c.Name).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Step1(Step1ViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Step1");
            return RedirectToAction("Step2", new { city = model.City, housingType = model.HousingType });
        }

        // ==================== ЭКРАН 2 ====================
        [HttpGet]
        public IActionResult Step2(string city, string housingType)
        {
            return View(new Step2ViewModel { City = city, HousingType = housingType });
        }

        [HttpPost]
        public async Task<IActionResult> Step2(Step2ViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var cityData = await _db.Cities.FirstOrDefaultAsync(c => c.Name == model.City);

            var result = new SurveyResult
            {
                City               = model.City,
                HousingType        = model.HousingType,
                TransportType      = model.TransportType,
                FlightsPerYear     = model.FlightsPerYear,
                HeatingType        = model.HeatingType,
                AcUsageMonths      = model.AcUsageMonths,
                HasWaterMeter      = model.HasWaterMeter,
                HasLedLights       = model.HasLedLights,
                ShoppingFrequency  = model.ShoppingFrequency,
                HasRecycling       = model.HasRecycling,
                // ✅ Микро-әдеттер
                CoffeeCupsPerMonth   = model.CoffeeCupsPerMonth,
                ShoppingTripsPerWeek = model.ShoppingTripsPerWeek
            };

            _calculator.CalculateScores(result, cityData);

            _db.SurveyResults.Add(result);
            await _db.SaveChangesAsync();

            return RedirectToAction("Result", new { id = result.Id });
        }

        // ==================== ЭКРАН 3 ====================
        [HttpGet]
        public async Task<IActionResult> Result(int id)
        {
            var result = await _db.SurveyResults.FirstOrDefaultAsync(r => r.Id == id);
            if (result == null) return RedirectToAction("Step1");

            var cityData = await _db.Cities.FirstOrDefaultAsync(c => c.Name == result.City);

            var (tumblerKg, shopperKg, message) =
                _calculator.CalculateMicroImpact(result, result.City);

            var model = new ResultViewModel
            {
                Result = result,
                City   = cityData ?? new CityData(),
                GreenEnergyImpact     = _calculator.CalculateSimulation(result, cityData, true,  false, false),
                PublicTransportImpact = _calculator.CalculateSimulation(result, cityData, false, true,  false),
                LocalProductionImpact = _calculator.CalculateSimulation(result, cityData, false, false, true),
                TumblerImpact        = tumblerKg,
                ShopperImpact        = shopperKg,
                MicroActionMessage   = message
            };

            return View(model);
        }

        // ==================== AJAX: Жүйелік симулятор ====================
        [HttpGet]
        public async Task<IActionResult> GetSimulationScore(
            int id, bool greenEnergy, bool publicTransport, bool localProduction)
        {
            var result = await _db.SurveyResults.FirstOrDefaultAsync(r => r.Id == id);
            if (result == null) return Json(new { success = false });

            var cityData = await _db.Cities.FirstOrDefaultAsync(c => c.Name == result.City);

            double newScore  = _calculator.CalculateSimulation(result, cityData, greenEnergy, publicTransport, localProduction);
            double reduction = Math.Round(result.TotalScore - newScore, 1);

            return Json(new
            {
                success      = true,
                score        = newScore,
                reduction    = reduction,
                coalPercent  = cityData?.CoalEnergyPercent ?? 50.0
            });
        }

        // ==================== AJAX: Микро-симулятор ====================
        [HttpGet]
        public async Task<IActionResult> GetMicroScore(int id, bool tumbler, bool shopper)
        {
            var result = await _db.SurveyResults.FirstOrDefaultAsync(r => r.Id == id);
            if (result == null) return Json(new { success = false });

            double plasticSaved = 0;
            double remaining    = result.TotalPlasticKg;

            if (tumbler) { plasticSaved += result.AnnualCoffeePlasticGrams / 1000.0; }
            if (shopper) { plasticSaved += result.AnnualBagPlasticGrams    / 1000.0; }

            plasticSaved = Math.Round(plasticSaved, 2);
            remaining    = Math.Round(Math.Max(0, result.TotalPlasticKg - plasticSaved), 2);

            int bagsAvoided   = shopper  ? result.ShoppingTripsPerWeek * 52 : 0;
            int cupsAvoided   = tumbler  ? result.CoffeeCupsPerMonth * 12   : 0;

            string message = (tumbler || shopper)
                ? $"Тек осы әдеттермен жылына <b>{plasticSaved} кг пластик</b> үнемдедіңіз" +
                  (cupsAvoided > 0  ? $" ({cupsAvoided} пластик стақан" : "") +
                  (bagsAvoided  > 0 ? $"{(cupsAvoided > 0 ? " + " : " (")}{bagsAvoided} пакет)" : (cupsAvoided > 0 ? ")" : "")) + "!"
                : "";

            return Json(new
            {
                success      = true,
                plasticSaved,
                remaining,
                bagsAvoided,
                cupsAvoided,
                message
            });
        }
    }
}
