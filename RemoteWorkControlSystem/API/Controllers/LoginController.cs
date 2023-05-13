using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        private readonly IConfiguration _config;

        public LoginController(ILoginService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Missing login details");
            }
            LoginResponse? loginResponse;
            try
            {
                loginResponse = await _service.LoginAsync(loginRequest);
            }
            catch
            {
                return BadRequest($"Invalid credentials");
            }


            if (loginResponse == null)
            {
                return BadRequest($"Invalid credentials");
            }
            return Ok(loginResponse);
        }
    }
    
}
