using Microsoft.AspNetCore.Mvc;
using ReservasBackend.Dtos;
using ReservasBackend.Services;

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

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (Success, Message) = await _userService.RegisterAsync(dto);

            if (!Success)
                return BadRequest(new { mensaje = Message });

            return Ok(new { mensaje = Message });
        }

        // Método GET para probar que el controlador funciona
       [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
            {
                var usuarios = await _userService.GetAllAsync();
                 return Ok(usuarios);
            }
    }
}
