using reservabackend.Models;
using ReservasBackend.Dtos;
using ReservasBackend.Repositories;

namespace ReservasBackend.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _repositorio;
        private readonly IUserRepository _userRepository;

        public DisponibilidadService(IDisponibilidadRepository repositorio, IUserRepository userRepository)
        {
            _repositorio = repositorio;
            _userRepository = userRepository;
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

            await _repositorio.ActualizarAsync(existente);
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

            disponibilidad.Estado = "Reservado";
            disponibilidad.PacienteId = pacienteId; // Asegurate de que exista este campo en el modelo
            await _repositorio.ActualizarAsync(disponibilidad);
            return true;
        }

        public async Task<bool> CancelarTurnoAsync(int disponibilidadId)
        {
            var disponibilidad = await _repositorio.ObtenerPorIdAsync(disponibilidadId);
            if (disponibilidad == null || disponibilidad.Estado != "Reservado")
                return false;

            disponibilidad.Estado = "Disponible";
            disponibilidad.PacienteId = null;
            await _repositorio.ActualizarAsync(disponibilidad);
            return true;
        }
        public async Task<int> ContarTurnosDisponiblesAsync(int profesionalId)
        {
            var turnos = await _repositorio.ObtenerPorProfesionalAsync(profesionalId);
            return turnos.Count(t => t.Estado == "Disponible");
        }

        public async Task<List<DisponibilidadDto>> ObtenerDisponiblesAsync()
        {
            var disponibles = await _repositorio.ObtenerDisponiblesAsync();

            List<DisponibilidadDto> listDisponibilidad = new List<DisponibilidadDto>();

            foreach (var disponible in disponibles)
            {
                Usuario profesional = await _userRepository.GetById(disponible.ProfesionalId);

                DisponibilidadDto dto = new DisponibilidadDto()
                {
                    Id = disponible.Id,
                    FechaHoraInicio = disponible.FechaHoraInicio,
                    FechaHoraFin = disponible.FechaHoraFin,
                    Estado = disponible.Estado,
                    NombreProfesional = profesional.Nombre,
                    ApellidoProfesional = profesional.Apellido,
                    Especialidad = profesional.Especialidad
                };

                listDisponibilidad.Add(dto);
            }

            return listDisponibilidad;
        }
        
        public async Task<IEnumerable<Disponibilidad>> ObtenerPorPacienteAsync(int pacienteId)
    {
    // Aquí necesitas que el repositorio tenga el método correspondiente, que devuelva la lista filtrada por pacienteId
    return await _repositorio.ObtenerPorPacienteAsync(pacienteId);
    }

       
    }
}
