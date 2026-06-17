using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcoSystem.Models
{
    public class SurveyResult
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Экран 1
        public string City { get; set; } = "";
        public string HousingType { get; set; } = "";

        // А блок: Мобильділік
        public string TransportType { get; set; } = "";
        public int FlightsPerYear { get; set; }

        // Б блок: Тұрмыс
        public string HeatingType { get; set; } = "";
        public int AcUsageMonths { get; set; }
        public bool HasWaterMeter { get; set; }
        public bool HasLedLights { get; set; }

        // В блок: Тұтыну
        public string ShoppingFrequency { get; set; } = "";
        public bool HasRecycling { get; set; }

        // ✅ ЖАҢА: Микро-әдеттер блогы
        public int CoffeeCupsPerMonth { get; set; }      // Айына сыртта ішетін кофе саны
        public int ShoppingTripsPerWeek { get; set; }     // Аптасына дүкенге бару саны

        // Нәтиже көрсеткіштері
        public double TotalScore { get; set; }
        public double MobilityScore { get; set; }
        public double LivingScore { get; set; }
        public double ConsumptionScore { get; set; }
        public int Co2Trees { get; set; }
        public double WaterPools { get; set; }
        public string SystemTag { get; set; } = "";

        // ✅ ЖАҢА: Микро-әдеттердің нақты физикалық салмағы
        public double AnnualCoffeePlasticGrams { get; set; }   // Жылына кофеден пластик (гр)
        public double AnnualBagPlasticGrams { get; set; }      // Жылына пакеттен пластик (гр)
        public double TotalPlasticKg { get; set; }             // Жиыны (кг)
    }

    public class CityData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Region { get; set; } = "";
        public double CoalEnergyPercent { get; set; }
        public double PrivateGasPercent { get; set; }
        public double PublicDieselPercent { get; set; }
    }

    public class Step1ViewModel
    {
        [Required(ErrorMessage = "Қаланы таңдаңыз")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "Тұрғын үй түрін таңдаңыз")]
        public string HousingType { get; set; } = "";

        public List<CityData> Cities { get; set; } = new();
    }

    public class Step2ViewModel
    {
        [Required]
        public string TransportType { get; set; } = "";

        [Range(0, 50)]
        public int FlightsPerYear { get; set; }

        [Required]
        public string HeatingType { get; set; } = "";

        [Range(0, 12)]
        public int AcUsageMonths { get; set; }

        public bool HasWaterMeter { get; set; }
        public bool HasLedLights { get; set; }

        [Required]
        public string ShoppingFrequency { get; set; } = "";

        public bool HasRecycling { get; set; }

        // ✅ ЖАҢА: Микро-әдеттер
        [Range(0, 100)]
        public int CoffeeCupsPerMonth { get; set; }

        [Range(0, 20)]
        public int ShoppingTripsPerWeek { get; set; }

        // Алдыңғы қадамнан
        public string City { get; set; } = "";
        public string HousingType { get; set; } = "";
    }

    public class ResultViewModel
    {
        public SurveyResult Result { get; set; } = new();
        public CityData City { get; set; } = new();

        // Жүйелік симулятор (бұрынғы)
        public double GreenEnergyImpact { get; set; }
        public double PublicTransportImpact { get; set; }
        public double LocalProductionImpact { get; set; }

        // ✅ ЖАҢА: Микро-симулятор (нақты пластик граммен)
        public double TumblerImpact { get; set; }   // Термостақан қолданса үнемделетін пластик (кг)
        public double ShopperImpact { get; set; }   // Шоппер қолданса үнемделетін пластик (кг)

        // ✅ ЖАҢА: Ынталандырушы хабарлама
        public string MicroActionMessage { get; set; } = "";
    }
}
