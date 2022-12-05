using HouseRentingSystem.Areas.Admin.Models;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    public class HouseController : BaseController
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HouseController(IHouseService _houseService, IAgentService _agentService)
        {
            this.houseService = _houseService;
            this.agentService = _agentService;
        }

        public async Task<IActionResult> Mine()
        {
            var myHouses = new MyHousesViewModel();
            var adminId = User.Id();
            var agentId = await agentService.GetAgentId(adminId);

            myHouses.RentedHouses = await houseService.AllHousesByUserId(adminId);
            myHouses.AddedHouses = await houseService.AllHousesByAgentId(agentId);

            return View(myHouses);
        }
    }
}
