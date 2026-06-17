using System;
using EcoSystem.Models;

namespace EcoSystem.Services
{
    public class ScoreCalculator
    {
        // =====================================================
        // ФИЗИКАЛЫҚ КОНСТАНТТАР
        // =====================================================
        private const double CoffeePlasticPerCup   = 15.0;  // 1 пластик стақан = 15 гр
        private const double BagPlasticPerTrip      = 8.0;   // 1 дүкен пакеті = 8 гр полиэтилен
        private const double WeeksPerYear           = 52.0;
        private const double MonthsPerYear          = 12.0;

        /// <summary>
        /// Негізгі индекс + микро-пластик есебін бір мезетте есептейді
        /// </summary>
        public void CalculateScores(SurveyResult result, CityData? city)
        {
            double coalPercent    = city?.CoalEnergyPercent  ?? 50.0;
            double privateGas     = city?.PrivateGasPercent  ?? 15.0;
            double publicDiesel   = city?.PublicDieselPercent ?? 45.0;

            // ==========================================
            // 🚗 А БЛОК: МОБИЛЬДІЛІК
            // ==========================================
            double mobilityBase = result.TransportType switch
            {
                "car_petrol"   => 30.0,
                "car_gas"      => 25.0 - (privateGas / 100.0 * 10.0),
                "car_electric" => 10.0,
                "public"       => 5.0 + (publicDiesel / 100.0 * 5.0),
                _              => 2.0   // велосипед / жаяу
            };

            double flightImpact = Math.Min(result.FlightsPerYear * 3.0, 10.0);
            result.MobilityScore = Math.Round(mobilityBase + flightImpact, 1);

            // ==========================================
            // 🏠 Б БЛОК: ТҰРМЫС
            // ==========================================
            double livingBase = result.HeatingType switch
            {
                "central"     => 18.0,
                "house_stove" => 22.0,
                "none"        => 5.0,
                _             => 18.0
            };
            livingBase += result.AcUsageMonths * 0.8;
            if (result.HasWaterMeter) livingBase -= 3.0;
            if (result.HasLedLights)  livingBase -= 2.0;
            livingBase += coalPercent * 0.08;
            result.LivingScore = Math.Round(livingBase, 1);

            // ==========================================
            // 🛍 В БЛОК: ТҰТЫНУ
            // ==========================================
            double consumptionBase = result.ShoppingFrequency switch
            {
                "rare"     => 5.0,
                "seasonal" => 12.0,
                "frequent" => 22.0,
                _          => 12.0
            };
            if (!result.HasRecycling) consumptionBase += 3.0;
            result.ConsumptionScore = Math.Round(consumptionBase, 1);

            // ==========================================
            // 📊 ЖАЛПЫ ИНДЕКС
            // ==========================================
            double total = result.MobilityScore + result.LivingScore + result.ConsumptionScore;
            result.TotalScore = Math.Round(Math.Clamp(total, 0.0, 100.0), 1);

            // ==========================================
            // 🌳 МЕТАФОРАЛАР
            // ==========================================
            double co2Tons = result.TotalScore * 0.4;
            result.Co2Trees   = (int)Math.Ceiling(co2Tons * 1000.0 / 22.0);
            result.WaterPools = Math.Round(result.TotalScore * 50.0 * 365.0 / 2_500_000.0, 2);

            result.SystemTag = result.TotalScore switch
            {
                <= 35 => "Төменгі экологиялық қысым. Сіздің өмір салтыңыз тұрақтылық нормаларына сай.",
                <= 60 => "Орташа қысым. Нәтижені жақсарту үшін қала инфрақұрылымын жасылдандыру қажет.",
                _     => "Жоғары экологиялық қысым! Бұл көрсеткішке аймағыңыздағы көмір мен дизельдің жоғары үлесі тікелей әсер етіп тұр."
            };

            // ==========================================
            // ☕🛍 МИКРО-ПЛАСТИК ЕСЕБІ (жаңа)
            // ==========================================
            result.AnnualCoffeePlasticGrams =
                result.CoffeeCupsPerMonth * MonthsPerYear * CoffeePlasticPerCup;

            result.AnnualBagPlasticGrams =
                result.ShoppingTripsPerWeek * WeeksPerYear * BagPlasticPerTrip;

            result.TotalPlasticKg = Math.Round(
                (result.AnnualCoffeePlasticGrams + result.AnnualBagPlasticGrams) / 1000.0, 2);
        }

        /// <summary>
        /// Жүйелік симулятор: индекс қаншаға өзгереді?
        /// </summary>
        public double CalculateSimulation(SurveyResult result, CityData? city,
            bool greenEnergy, bool publicTransport, bool localProduction)
        {
            double score       = result.TotalScore;
            double coalPercent = city?.CoalEnergyPercent ?? 50.0;

            if (greenEnergy)      score -= coalPercent * 0.15;
            if (publicTransport)  score -= result.MobilityScore * 0.4;
            if (localProduction)  score -= result.ConsumptionScore * 0.3;

            return Math.Round(Math.Max(0.0, score), 1);
        }

        /// <summary>
        /// Микро-симулятор: термостақан/шоппер қолданса нақты пластик үнемі (кг)
        /// </summary>
        public (double tumblerSaved, double shopperSaved, string message)
            CalculateMicroImpact(SurveyResult result, string cityName)
        {
            double tumblerKg = Math.Round(result.AnnualCoffeePlasticGrams / 1000.0, 2);
            double shopperKg = Math.Round(result.AnnualBagPlasticGrams   / 1000.0, 2);
            double totalKg   = Math.Round(tumblerKg + shopperKg, 2);

            // Пластик дананы есептеу
            int bagCount     = result.ShoppingTripsPerWeek * 52;
            int coffeeCount  = result.CoffeeCupsPerMonth * 12;

            string message = totalKg > 0
                ? $"Керемет! Сіз тек термостақан мен эко-дорба қолдану арқылы " +
                  $"{cityName} қаласында жылына <strong>{totalKg} килограмм таза пластиктің</strong> " +
                  $"қоқыс полигонына түсуіне жол бермедіңіз! " +
                  $"Бұл {coffeeCount} пластик стақан мен {bagCount} полиэтилен пакетке тең."
                : "Сіз микро-пластик тұтынуды толық азайтқансыз — өте жақсы!";

            return (tumblerKg, shopperKg, message);
        }
    }
}
