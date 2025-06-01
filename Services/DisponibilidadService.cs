using reservabackend.Models;
using ReservasBackend.Repositories;

namespace ReservasBackend.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _repositorio;

        public DisponibilidadService(IDisponibilidadRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Disponibilidad>> ObtenerTodasAsync()
        {
            return await _repositorio.ObtenerTodasAsync();
        }

        public async Task<Disponibilidad> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Disponibilidad>> ObtenerPorProfesionalAsync(int profesionalId)
        {
            return await _repositorio.ObtenerPorProfesionalAsync(profesionalId);
        }

        public async Task<bool> CrearDisponibilidadAsync(Disponibilidad disponibilidad)
        {
            if (disponibilidad.FechaHoraInicio >= disponibilidad.FechaHoraFin)
                return false;

            bool existeSolapamiento = await _repositorio.ExisteSolapamientoAsync(
                disponibilidad.FechaHoraInicio,
                disponibilidad.FechaHoraFin,
                disponibilidad.ProfesionalId);

            if (existeSolapamiento)
                return false;

            disponibilidad.Estado = "Disponible"; // Por si no vino seteado
            await _repositorio.AgregarAsync(disponibilidad);
            return true;
        }
    
        public async Task<bool> ActualizarDisponibilidadAsync(Disponibilidad disponibilidad)
        {
        var existente = await _repositorio.ObtenerPorIdAsync(disponibilidad.Id);
        if (existente == null)
        return false;

        // Validar que solo se pueda modificar si está Disponible
        if (existente.Estado != "Disponible")
        return false;

        // Validar fechas
        if (disponibilidad.FechaHoraInicio >= disponibilidad.FechaHoraFin)
        return false;
        existente.Duracion = disponibilidad.Duracion;
        existente.Estado = disponibilidad.Estado;
        existente.FechaHoraInicio = disponibilidad.FechaHoraInicio;
        existente.FechaHoraFin = disponibilidad.FechaHoraFin;

     // Podrías agregar aquí validación de solapamientos si quieres

        await _repositorio.ActualizarAsync(disponibilidad);
        return true;
        }


        public async Task<bool> EliminarDisponibilidadAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
            return true;
        }

        // NUEVOS: Reservar y Cancelar turno
        public async Task<bool> ReservarTurnoAsync(int disponibilidadId, int pacienteId)
        {
            var disponibilidad = await _repositorio.ObtenerPorIdAsync(disponibilidadId);
            if (disponibilidad == null || disponibilidad.Estado != "Disponible")
                return false;

            disponibilidad.Estado = "Ocupado";
            disponibilidad.PacienteId = pacienteId; // Asegurate de que exista este campo en el modelo
            await _repositorio.ActualizarAsync(disponibilidad);
            return true;
        }

        public async Task<bool> CancelarTurnoAsync(int disponibilidadId)
        {
            var disponibilidad = await _repositorio.ObtenerPorIdAsync(disponibilidadId);
            if (disponibilidad == null || disponibilidad.Estado != "Ocupado")
                return false;

            disponibilidad.Estado = "Disponible";
            disponibilidad.PacienteId = null;
            await _repositorio.ActualizarAsync(disponibilidad);
            return true;
        }
    }
}
