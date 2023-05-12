﻿using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/")]
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _service;

        public ProjectMemberController(IProjectMemberService service)
        {
            _service = service;
        }

        // GET: api/<ProjectMemberController>
        [HttpGet]
        [Route("ProjectMembers")]
        public async Task<ActionResult<IEnumerable<ProjectMemberModel>>> Get()
        {
            var projectMembers = await _service.GetAllAsync();

            if (projectMembers == null)
            {
                return NotFound();
            }
            else
            {

                return new ObjectResult(projectMembers);
            }

        }

        // GET api/<ProjectMemberController>/5
        [HttpGet("ProjectMember/{id}")]
        public async Task<ActionResult<ProjectMemberModel>> GetById(int id)
        {
            var projectMember = await _service.GetByIdAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(projectMember);
            }
        }



        // POST api/ProjectMember
        [HttpPost("ProjectMember")]
        public async Task<ActionResult> Post([FromBody] ProjectMemberModel projectMember)
        {
            if (projectMember == null)
            {
                return BadRequest();
            }
            try
            {
                await _service.AddAsync(projectMember);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(projectMember);

        }

        // PUT api/<ProjectMemberController>/5
        [HttpPut("ProjectMember/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProjectMemberModel value)
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

        // DELETE api/<ProjectMemberController>/5
        [HttpDelete("ProjectMember/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var projectMember = await _service.GetByIdAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok(projectMember);
        }
    }
}