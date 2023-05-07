using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/")]
    [ApiController]
    public class WorkSessionController : ControllerBase
    {
        private readonly IWorkSessionService _service;

        public WorkSessionController(IWorkSessionService service)
        {
            _service = service;
        }

        // GET: api/<WorkSessionController>
        [HttpGet]
        [Route("WorkSessions")]
        public async Task<ActionResult<IEnumerable<WorkSessionModel>>> Get()
        {
            var workSessions = await _service.GetAllAsync();

            if (workSessions == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(workSessions);
            }

        }

        // GET api/<WorkSessionController>/5
        [HttpGet("WorkSession/{id}")]
        public async Task<ActionResult<WorkSessionModel>> GetById(int id)
        {
            var workSession = await _service.GetByIdAsync(id);
            if (workSession == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(workSession);
            }
        }



        // POST api/WorkSessionPhoto
        [HttpPost("WorkSession")]
        public async Task<ActionResult> Post([FromBody] WorkSessionModel workSession)
        {
            if (workSession == null)
            {
                return BadRequest();
            }
            try
            {
                await _service.AddAsync(workSession);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(workSession);

        }

        // PUT api/<WorkSessionController>/5
        [HttpPut("WorkSession/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] WorkSessionModel value)
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

        // DELETE api/<WorkSessionController>/5
        [HttpDelete("WorkSession/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var workSession = await _service.GetByIdAsync(id);
            if (workSession == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok(workSession);
        }
    }
    }
