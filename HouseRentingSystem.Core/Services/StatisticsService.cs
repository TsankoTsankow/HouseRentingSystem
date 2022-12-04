using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using HouseRentingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext context;

        public StatisticsService(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            int totalHouses = await context.Houses
                .CountAsync(h => h.IsActive);
            int rentedHouses = await context.Houses
                .CountAsync(h => h.IsActive && h.RenterId != null);

            return new StatisticsServiceModel()
            {
                TotalHouses = totalHouses,
                TotalRents = rentedHouses
            };

        }
    }
}
