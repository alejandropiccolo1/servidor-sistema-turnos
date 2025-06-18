using reservabackend.Models;

namespace ReservasBackend.Repositories
{
    public interface IDisponibilidadRepository
    {
        Task<IEnumerable<Disponibilidad>> ObtenerTodasAsync();
        Task<Disponibilidad> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Disponibilidad disponibilidad);
        Task ActualizarAsync(Disponibilidad disponibilidad);
        Task EliminarAsync(int id);
        Task<IEnumerable<Disponibilidad>> ObtenerPorProfesionalAsync(int profesionalId);
        Task<bool> ExisteSolapamientoAsync(DateTime inicio, DateTime fin, int profesionalId);
        Task<IEnumerable<Disponibilidad>> ObtenerDisponiblesAsync();
        Task<IEnumerable<Disponibilidad>> ObtenerPorPacienteAsync(int pacienteId);
        Task<bool> ReservarTurnoAsync(int disponibilidadId, int pacienteId);


    }
}