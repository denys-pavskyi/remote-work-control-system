using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/")]
    [ApiController]
    public class EmployeeScreenActivityController : ControllerBase
    {
        private readonly IEmployeeScreenActivityService _service;

        public EmployeeScreenActivityController(IEmployeeScreenActivityService service)
        {
            _service = service;
        }

        // GET: api/<EmployeeScreenActivityController>
        [HttpGet]
        [Route("EmployeeScreenActivitys")]
        public async Task<ActionResult<IEnumerable<EmployeeScreenActivityModel>>> Get()
        {
            var employeeScreenActivities = await _service.GetAllAsync();

            if (employeeScreenActivities == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(employeeScreenActivities);
            }

        }

        // GET api/<EmployeeScreenActivityController>/5
        [HttpGet("EmployeeScreenActivity/{id}")]
        public async Task<ActionResult<EmployeeScreenActivityModel>> GetById(int id)
        {
            var employeeScreenActivity = await _service.GetByIdAsync(id);
            if (employeeScreenActivity == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(employeeScreenActivity);
            }
        }



        // POST api/EmployeeScreenActivityPhoto
        [HttpPost("EmployeeScreenActivity")]
        public async Task<ActionResult> Post([FromBody] EmployeeScreenActivityModel employeeScreenActivity)
        {
            if (employeeScreenActivity == null)
            {
                return BadRequest();
            }
            try
            {
                await _service.AddAsync(employeeScreenActivity);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(employeeScreenActivity);

        }

        // PUT api/<EmployeeScreenActivityController>/5
        [HttpPut("EmployeeScreenActivity/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmployeeScreenActivityModel value)
        {
            try
            {
                await _service.UpdateAsync(value);
            }
            catch
            {
                return BadRequest();
            }


            if (await _service.GetByIdAsync(id) == null)
            {
                return BadRequest();
            }

            return Ok(value);
        }

        // DELETE api/<EmployeeScreenActivityController>/5
        [HttpDelete("EmployeeScreenActivity/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employeeScreenActivity = await _service.GetByIdAsync(id);
            if (employeeScreenActivity == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok(employeeScreenActivity);
        }
    }
}