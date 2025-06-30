using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservabackend.Models;
using ReservasBackend.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IDisponibilidadService _disponibilidadService;

        public DisponibilidadController(IDisponibilidadService disponibilidadService)
        {
            _disponibilidadService = disponibilidadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDisponibilidades()
        {
            int profesionalId = ObtenerProfesionalIdDesdeToken();
            var disponibilidades = await _disponibilidadService.ObtenerPorProfesionalAsync(profesionalId);
            return Ok(disponibilidades);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var disponibilidad = await _disponibilidadService.ObtenerPorIdAsync(id);
            if (disponibilidad == null)
                return NotFound(new { success = false, message = "Disponibilidad no encontrada." });

            int profesionalId = ObtenerProfesionalIdDesdeToken();
            if (disponibilidad.ProfesionalId != profesionalId)
                return Unauthorized(new { success = false, message = "No tenés permiso para ver esta disponibilidad." });

            return Ok(disponibilidad);

        }
        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> ObtenerPorPaciente(int pacienteId)
        {
            // Aquí podés agregar validaciones según el rol o usuario logueado
            var disponibilidades = await _disponibilidadService.ObtenerPorPacienteAsync(pacienteId);
            return Ok(disponibilidades);
        }

        [HttpPost]
        public async Task<IActionResult> CrearDisponibilidad([FromBody] Disponibilidad disponibilidad)
        {
            int profesionalId = ObtenerProfesionalIdDesdeToken();
            disponibilidad.ProfesionalId = profesionalId;

            bool creado = await _disponibilidadService.CrearDisponibilidadAsync(disponibilidad);

            if (!creado)
                return BadRequest(new { success = false, message = "Error: La disponibilidad es inválida o hay solapamientos." });

            return Ok(new { success = true, message = "Disponibilidad creada correctamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarDisponibilidad(int id, [FromBody] Disponibilidad disponibilidad)
        {
            int profesionalId = ObtenerProfesionalIdDesdeToken();

            if (id != disponibilidad.Id)
                return BadRequest(new { success = false, message = "Id de disponibilidad no coincide." });

            if (disponibilidad.ProfesionalId != profesionalId)
                return Unauthorized(new { success = false, message = "No tenés permiso para modificar esta disponibilidad." });

            // Forzar el profesionalId que viene del token, para que no sea modificado desde el body
            disponibilidad.ProfesionalId = profesionalId;

            bool actualizado = await _disponibilidadService.ActualizarDisponibilidadAsync(disponibilidad);

            if (!actualizado)
                return BadRequest(new { success = false, message = "Error al actualizar disponibilidad." });

            return Ok(new { success = true, message = "Disponibilidad actualizada." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDisponibilidad(int id)
        {
            int profesionalId = ObtenerProfesionalIdDesdeToken();

            var disponibilidad = await _disponibilidadService.ObtenerPorIdAsync(id);
            if (disponibilidad == null)
                return NotFound(new { success = false, message = "No se encontró la disponibilidad." });

            if (disponibilidad.ProfesionalId != profesionalId)
                return Unauthorized(new { success = false, message = "No tenés permiso para eliminar esta disponibilidad." });

            bool eliminado = await _disponibilidadService.EliminarDisponibilidadAsync(id);

            if (!eliminado)
                return BadRequest(new { success = false, message = "Error al eliminar disponibilidad." });

            return Ok(new { success = true, message = "Disponibilidad eliminada." });
        }

        private int ObtenerProfesionalIdDesdeToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("Token inválido o sin claim de Id");
            return int.Parse(claim.Value);
        }

        [HttpGet("contador")]
        public async Task<IActionResult> ContarDisponibles()
        {
            int profesionalId = ObtenerProfesionalIdDesdeToken();
            int cantidad = await _disponibilidadService.ContarTurnosDisponiblesAsync(profesionalId);

            return Ok(new { cantidad });
        }
        [HttpGet("disponibles")]
        public async Task<IActionResult> ObtenerDisponibles()
        {
            var disponibles = await _disponibilidadService.ObtenerDisponiblesAsync();
            return Ok(disponibles);
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> CancelarTurno(int id)
        {
            var resultado = await _disponibilidadService.CancelarTurnoAsync(id);
            if (!resultado)
                return BadRequest("No se pudo cancelar el turno.");
            return Ok("Turno cancelado correctamente.");
        }
        [HttpPut("reservar/{disponibilidadId}")]
        [Authorize(Roles = "paciente")]
         public async Task<IActionResult> ReservarTurno(int disponibilidadId)
    {
         // 1) Obtener el pacienteId desde el token:
         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            return Unauthorized("Token inválido o sin identificador de usuario.");
            int pacienteId = int.Parse(claim.Value);

            // 2) Llamar al servicio:
            bool reservado = await _disponibilidadService.ReservarTurnoAsync(disponibilidadId, pacienteId);
        if (!reservado)
        return BadRequest("No se pudo reservar el turno. Puede que ya no esté disponible.");

        return Ok("Turno reservado correctamente.");
        }

    }
}
