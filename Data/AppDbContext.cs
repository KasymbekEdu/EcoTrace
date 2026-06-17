using Microsoft.EntityFrameworkCore;
using EcoSystem.Models;

namespace EcoSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<CityData> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Қазақстанның 3 мегаполисі мен 17 облысы (Шынайы ресми статистика — 20 қала/аймақ)
            modelBuilder.Entity<CityData>().HasData(
                // === МЕГАПОЛИСТЕР ===
                new CityData { Id = 1, Name = "Алматы қ.", Region = "Мегаполис", CoalEnergyPercent = 55, PrivateGasPercent = 15, PublicDieselPercent = 10 },
                new CityData { Id = 2, Name = "Астана қ.", Region = "Мегаполис", CoalEnergyPercent = 87, PrivateGasPercent = 6, PublicDieselPercent = 45 },
                new CityData { Id = 3, Name = "Шымкент қ.", Region = "Мегаполис", CoalEnergyPercent = 25, PrivateGasPercent = 47, PublicDieselPercent = 5 },

                // === ОБЛЫС ОРТАЛЫҚТАРЫ ===
                new CityData { Id = 4, Name = "Қарағанды", Region = "Қарағанды облысы", CoalEnergyPercent = 97, PrivateGasPercent = 5, PublicDieselPercent = 85 },
                new CityData { Id = 5, Name = "Павлодар", Region = "Павлодар облысы", CoalEnergyPercent = 99, PrivateGasPercent = 4, PublicDieselPercent = 82 },
                new CityData { Id = 6, Name = "Өскемен", Region = "Шығыс Қазақстан облысы", CoalEnergyPercent = 65, PrivateGasPercent = 3, PublicDieselPercent = 90 },
                new CityData { Id = 7, Name = "Семей", Region = "Абай облысы", CoalEnergyPercent = 67, PrivateGasPercent = 4, PublicDieselPercent = 88 },
                new CityData { Id = 8, Name = "Талдықорған", Region = "Жетісу облысы", CoalEnergyPercent = 48, PrivateGasPercent = 20, PublicDieselPercent = 75 },
                new CityData { Id = 9, Name = "Жезқазған", Region = "Ұлытау облысы", CoalEnergyPercent = 92, PrivateGasPercent = 5, PublicDieselPercent = 90 },
                new CityData { Id = 10, Name = "Тараз", Region = "Жамбыл облысы", CoalEnergyPercent = 30, PrivateGasPercent = 35, PublicDieselPercent = 40 },
                new CityData { Id = 11, Name = "Қызылорда", Region = "Қызылорда облысы", CoalEnergyPercent = 20, PrivateGasPercent = 43, PublicDieselPercent = 10 },
                new CityData { Id = 12, Name = "Түркістан", Region = "Түркістан облысы", CoalEnergyPercent = 27, PrivateGasPercent = 45, PublicDieselPercent = 35 },
                new CityData { Id = 13, Name = "Орал", Region = "Батыс Қазақстан облысы", CoalEnergyPercent = 0, PrivateGasPercent = 40, PublicDieselPercent = 30 },
                new CityData { Id = 14, Name = "Ақтөбе", Region = "Ақтөбе облысы", CoalEnergyPercent = 0, PrivateGasPercent = 43, PublicDieselPercent = 15 },
                new CityData { Id = 15, Name = "Атырау", Region = "Атырау облысы", CoalEnergyPercent = 0, PrivateGasPercent = 50, PublicDieselPercent = 25 },
                new CityData { Id = 16, Name = "Ақтау", Region = "Маңғыстау облысы", CoalEnergyPercent = 0, PrivateGasPercent = 79, PublicDieselPercent = 20 },
                new CityData { Id = 17, Name = "Қостанай", Region = "Қостанай облысы", CoalEnergyPercent = 87, PrivateGasPercent = 5, PublicDieselPercent = 78 },
                new CityData { Id = 18, Name = "Петропавл", Region = "Солтүстік Қазақстан облысы", CoalEnergyPercent = 95, PrivateGasPercent = 3, PublicDieselPercent = 92 },
                new CityData { Id = 19, Name = "Көкшетау", Region = "Ақмола облысы", CoalEnergyPercent = 97, PrivateGasPercent = 4, PublicDieselPercent = 85 },
                new CityData { Id = 20, Name = "Қонаев", Region = "Алматы облысы", CoalEnergyPercent = 57, PrivateGasPercent = 22, PublicDieselPercent = 65 }
            );
        }
    }
}