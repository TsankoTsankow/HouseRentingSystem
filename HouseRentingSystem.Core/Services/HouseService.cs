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

        public async Task<IEnumerable<HouseCategoryModel>> AllCategories()
        {
            return await context.Categories
                .OrderByDescending(c => c.Name)
                .Select(c => new HouseCategoryModel()
                {
                    Name = c.Name,
                    Id = c.Id
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExists(int CategoryId)
        {
            return await context.Categories.AnyAsync(c => c.Id == CategoryId);
        }

        public async Task<int> Create(HouseModel model, int agentId)
        {
            var house = new House()
            {
                Address = model.Address,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth, 
                Title = model.Title,
                AgentId = agentId
            };

            await context.AddAsync(house);
            await context.SaveChangesAsync();

            return house.Id;
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
