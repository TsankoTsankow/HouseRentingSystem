using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService _statistics)
        {
            this.statistics = _statistics;
        }

        [HttpGet]
        public async Task<StatisticsServiceModel> GetStatistics()
        {
            return await statistics.Total();
        }
    }
}
