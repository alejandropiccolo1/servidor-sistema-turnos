// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using reservabackend.Models;
// using ReservasBackend.Services;
// using System.Security.Claims;
// using System.Threading.Tasks;

// namespace ReservasBackend.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     [Authorize]  // Asegura que el paciente esté autenticado
//     public class TurnosController : ControllerBase
//     {
//         private readonly IDisponibilidadService _disponibilidadService;

//         public TurnosController(IDisponibilidadService disponibilidadService)
//         {
//             _disponibilidadService = disponibilidadService;
//         }

//         // GET: api/turnos/disponibles
//         [HttpGet("disponibles")]
//         public async Task<IActionResult> ObtenerTurnosDisponibles()
//         {
//             var disponibles = await _disponibilidadService.ObtenerDisponiblesAsync();
//             return Ok(disponibles);
//         }

//         // POST: api/turnos/reservar/{disponibilidadId}
//         [HttpPost("reservar/{disponibilidadId}")]
//         public async Task<IActionResult> ReservarTurno(int disponibilidadId)
//         {
//             int pacienteId = ObtenerPacienteIdDesdeToken();

//             bool reservado = await _disponibilidadService.ReservarTurnoAsync(disponibilidadId, pacienteId);

//             if (!reservado)
//                 return BadRequest(new { success = false, message = "No se pudo reservar el turno. Puede estar ya reservado o no existir." });

//             return Ok(new { success = true, message = "Turno reservado correctamente." });
//         }

//         // POST: api/turnos/cancelar/{disponibilidadId}
//         [HttpPost("cancelar/{disponibilidadId}")]
//         public async Task<IActionResult> CancelarTurno(int disponibilidadId)
//         {
//             int pacienteId = ObtenerPacienteIdDesdeToken();

//             var disponibilidad = await _disponibilidadService.ObtenerPorIdAsync(disponibilidadId);
//             if (disponibilidad == null)
//                 return NotFound(new { success = false, message = "Turno no encontrado." });

//             // Validar que solo pueda cancelar si es el mismo paciente que reservó
//             if (disponibilidad.PacienteId != pacienteId)
//                 return Unauthorized(new { success = false, message = "No tenés permiso para cancelar este turno." });

//             bool cancelado = await _disponibilidadService.CancelarTurnoAsync(disponibilidadId);

//             if (!cancelado)
//                 return BadRequest(new { success = false, message = "No se pudo cancelar el turno." });

//             return Ok(new { success = true, message = "Turno cancelado correctamente." });
//         }

//         private int ObtenerPacienteIdDesdeToken()
//         {
//             var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//             if (claim == null)
//                 throw new UnauthorizedAccessException("Token inválido o sin claim de Id");
//             return int.Parse(claim.Value);
//         }
//     }
// }
