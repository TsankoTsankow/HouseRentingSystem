using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Exceptions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly ApplicationDbContext context;
        private readonly IGuard guard;
        private readonly ILogger logger;

        public HouseService(
            ApplicationDbContext _context,
            IGuard _guard,
            ILogger<HouseService> _logger)
        {
            this.context = _context;
            this.guard = _guard;
            this.logger = _logger;
        }

        public async Task<HousesQueryModel> All(string? category = null, string? searchTerm = null, HouseSorting sorting = HouseSorting.Newest, int currentPage = 1, int housesPerPage = 1)
        {
            var housesQuery = context.Houses
                .Where(h =>h.IsActive == true)
                .AsQueryable();
            var result = new HousesQueryModel();

            if (string.IsNullOrEmpty(category) == false)
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                housesQuery = housesQuery
                    .Where(h => EF.Functions.Like(h.Title.ToLower(), searchTerm) ||
                         EF.Functions.Like(h.Address.ToLower(), searchTerm) ||
                         EF.Functions.Like(h.Description.ToLower(), searchTerm));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRented => housesQuery
                    .OrderBy(h => h.RenterId),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            result.Houses = await housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel()
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();

            result.TotalHouses = await housesQuery.CountAsync();

            return result;
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

        public async  Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await context.Categories
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int Id)
        {
            return await context.Houses
                .Where(h => h.IsActive == true)
                .Where(a => a.AgentId == Id)
                .Select(a => new HouseServiceModel()
                {
                    Address = a.Address,
                    Id = a.Id, 
                    ImageUrl= a.ImageUrl,   
                    IsRented= a.RenterId != null,
                    PricePerMonth= a.PricePerMonth,
                    Title = a.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string Id)
        {
            return await context.Houses
                .Where(h => h.IsActive == true)
                .Where(a => a.RenterId == Id)
                .Select(a => new HouseServiceModel()
                {
                    Address = a.Address,
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    IsRented = a.RenterId != null,
                    PricePerMonth = a.PricePerMonth,
                    Title = a.Title
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

            try
            {
                await context.AddAsync(house);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database failure, failed to saved the info in Create method");
                throw new ApplicationException("Database failed to save info", ex);
            }
            
            return house.Id;
        }

        public async Task Delete(int houseId)
        {
            var house = await context.Houses.FirstAsync(c => c.Id == houseId);

            house.IsActive = false;

            await context.SaveChangesAsync();
        }

        public async Task Edit(int houseId, HouseModel model)
        {
            var house = await context.Houses.FirstAsync(h => h.Id == houseId);

            house.PricePerMonth = model.PricePerMonth;
            house.ImageUrl = model.ImageUrl;
            house.Description = model.Description;
            house.Title = model.Title;
            house.Address = model.Address; 
            house.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await context.Houses
                .FirstAsync(h => h.Id == houseId))
                .CategoryId;
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            bool result = false;

            var house = await context.Houses
                .Where(h => h.IsActive == true)
                .Where(h => h.Id == houseId)
                .Include(h => h.Agent)
                .FirstOrDefaultAsync();

            if (house?.Agent != null && house.Agent.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<HouseDetailsModel> HouseDetailsById(int Id)
        {
            return await context.Houses
                .Where(h => h.IsActive == true)
                .Where(c => c.Id == Id)
                .Select(h => new HouseDetailsModel()
                {
                    Address = h.Address,
                    Category = h.Category.Name,
                    Description = h.Description,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth, 
                    Title = h.Title,
                    Agent = new Models.Agent.AgentServiceModel()
                    {
                        Email = h.Agent.User.Email, 
                        PhoneNumber = h.Agent.PhoneNumer
                    }
                })
                .FirstAsync();
        }

        public async Task<bool> HouseExists(int Id)
        {
            return await context.Houses.AnyAsync(h => h.Id == Id && h.IsActive == true);
        }

        public async Task<bool> IsRented(int houseId)
        {
            return (await context.Houses.FirstAsync(h => h.Id == houseId)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string currentUserId)
        {
            bool result = false;

            var house = await context.Houses
                .Where(h => h.IsActive == true)
                .Where(h => h.Id == houseId)
                .FirstOrDefaultAsync();

            if (house != null && house.RenterId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<HouseHomeModel>> LastThreeHouses()
        {
            return await context.Houses
                .Where(h => h.IsActive == true)
                .OrderByDescending(x => x.Id)
                .Take(3)
                .Select(h => new HouseHomeModel()
                {
                    Id = h.Id,
                    ImageUrl = h.ImageUrl, 
                    Title = h.Title, 
                    Address = h.Address
                })
                .ToListAsync();
        }

        public async Task Leave(int houseId)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == houseId);

            guard.AgainstNull(house, "House can not be found");

            house.RenterId = null;

            await context.SaveChangesAsync();
        }

        public async Task Rent(int houseId, string currentUserId)
        {
            var house = await context.Houses
                .Where(h => h.IsActive == true)
                .FirstOrDefaultAsync(h => h.Id == houseId);

            if (house != null && house.RenterId != null)
            {
                throw new ArgumentException("House is already rented");
            }

            guard.AgainstNull(house, "House can not be found");

            house.RenterId = currentUserId;

            await context.SaveChangesAsync();
        }
    }
}
