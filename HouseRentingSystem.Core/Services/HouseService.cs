using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly ApplicationDbContext context;

        public HouseService(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<HouseHomeModel>> LastThreeHouses()
        {
            return await context.Houses
                .OrderByDescending(x => x.Id)
                .Take(3)
                .Select(h => new HouseHomeModel()
                {
                    Id = h.Id,
                    ImageUrl = h.ImageUrl, 
                    Title = h.Title
                })
                .ToListAsync();
        }
    }
}
