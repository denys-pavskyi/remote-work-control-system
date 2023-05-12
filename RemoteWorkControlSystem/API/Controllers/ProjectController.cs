using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        // GET: api/<ProjectController>
        [HttpGet]
        [Route("Projects")]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> Get()
        {
            var projects = await _service.GetAllAsync();

            if (projects == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(projects);
            }

        }

        // GET api/<ProjectController>/5
        [HttpGet("Project/{id}")]
        public async Task<ActionResult<ProjectModel>> GetById(int id)
        {
            var project = await _service.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(project);
            }
        }



        // POST api/ProjectPhoto
        [HttpPost("Project")]
        public async Task<ActionResult> Post([FromBody] ProjectModel project)
        {
            if (project == null)
            {
                return BadRequest();
            }
            try
            {
                await _service.AddAsync(project);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(project);

        }

        // PUT api/<ProjectController>/5
        [HttpPut("Project/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProjectModel value)
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

        // DELETE api/<ProjectController>/5
        [HttpDelete("Project/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var project = await _service.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok(project);
        }
    }
}
