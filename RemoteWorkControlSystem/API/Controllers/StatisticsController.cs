using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private readonly IStatisticsService _service;

        public StatisticsController(IStatisticsService service)
        {
            _service = service;
        }

        


        // GET: api/<WorkSessionController>
        [HttpGet]
        [Route("Statistics")]
        public async Task<ActionResult<StatisticsData>> GetStatisticsByDataFrame(DateTime start, DateTime end, int projectMemberId)
        {

            var statisticsData = await _service.GetStatisticsByDataFrame(start, end, projectMemberId);

            if (statisticsData == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(statisticsData);
            }

        }
    }
}
