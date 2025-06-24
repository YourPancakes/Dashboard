using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendDashboard.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            db.Database.Migrate();
            if (db.Clients.Any()) return;

            var clients = new[]
            {
                new Client { Id = 1, Name = "Mantis", Email = "Mantis@example.com", BalanceT = 100m },
                new Client { Id = 2, Name = "FAForevor", Email = "FAForevor@example.com", BalanceT = 100m },
                new Client { Id = 3, Name = "SupCom", Email = "SupCom@example.com", BalanceT = 200m }
            };
            db.Clients.AddRange(clients);

            var payments = new[]
            {
                new Payment { Id = 1, ClientId = 1, AmountT = 100m, Timestamp = new DateTime(2025, 6, 20,  8,  0,  0, DateTimeKind.Utc) },
                new Payment { Id = 2, ClientId = 2, AmountT = 200m, Timestamp = new DateTime(2025, 6, 20,  9,  0,  0, DateTimeKind.Utc) },
                new Payment { Id = 3, ClientId = 3, AmountT = 300m, Timestamp = new DateTime(2025, 6, 20, 10,  0,  0, DateTimeKind.Utc) },
                new Payment { Id = 4, ClientId = 1, AmountT = 400m, Timestamp = new DateTime(2025, 6, 20, 11,  0,  0, DateTimeKind.Utc) },
                new Payment { Id = 5, ClientId = 2, AmountT = 500m, Timestamp = new DateTime(2025, 6, 20, 12,  0,  0, DateTimeKind.Utc) }
            };
            db.Payments.AddRange(payments);

            var rate = new Rate { Id = 1, CurrentRate = 10m, UpdatedAt = DateTime.UtcNow };
            db.Rates.Add(rate);

            db.SaveChanges();
        }
    }
}
