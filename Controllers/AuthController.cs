using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReservasBackend.Dtos;
using ReservasBackend.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReservasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistroDto dto)
    {
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var (success, message) = await _userService.RegisterAsync(dto);
    if (!success)
        return BadRequest(new { success = false, message });

    return Ok(new { success = true, message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {   
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var (success, message, token) = await _userService.LoginAsync(dto);

    if (!success)
        return Unauthorized(new { success = false, message });

    return Ok(new { success = true, message, token });
    }
    }
}
