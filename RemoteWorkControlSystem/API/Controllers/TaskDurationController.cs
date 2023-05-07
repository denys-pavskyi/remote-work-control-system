using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/")]
    [ApiController]
    public class TaskDurationController : ControllerBase
    {
        private readonly ITaskDurationService _service;

        public TaskDurationController(ITaskDurationService service)
        {
            _service = service;
        }

        // GET: api/<TaskDurationController>
        [HttpGet]
        [Route("TaskDurations")]
        public async Task<ActionResult<IEnumerable<TaskDurationModel>>> Get()
        {
            var taskDurations = await _service.GetAllAsync();

            if (taskDurations == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(taskDurations);
            }

        }

        // GET api/<TaskDurationController>/5
        [HttpGet("TaskDuration/{id}")]
        public async Task<ActionResult<TaskDurationModel>> GetById(int id)
        {
            var taskDuration = await _service.GetByIdAsync(id);
            if (taskDuration == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(taskDuration);
            }
        }



        // POST api/TaskDurationPhoto
        [HttpPost("TaskDuration")]
        public async Task<ActionResult> Post([FromBody] TaskDurationModel taskDuration)
        {
            if (taskDuration == null)
            {
                return BadRequest();
            }
            try
            {
                await _service.AddAsync(taskDuration);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(taskDuration);

        }

        // PUT api/<TaskDurationController>/5
        [HttpPut("TaskDuration/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TaskDurationModel value)
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

        // DELETE api/<TaskDurationController>/5
        [HttpDelete("TaskDuration/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var taskDuration = await _service.GetByIdAsync(id);
            if (taskDuration == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok(taskDuration);
        }
    }
    }
