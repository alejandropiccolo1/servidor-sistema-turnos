using reservabackend.Models;
using ReservasBackend.Repositories;

namespace ReservasBackend.Services
{
    public interface IDisponibilidadService
    {
        Task<IEnumerable<Disponibilidad>> ObtenerTodasAsync();
        Task<Disponibilidad> ObtenerPorIdAsync(int id);
        Task<bool> CrearDisponibilidadAsync(Disponibilidad disponibilidad);
        Task<bool> ActualizarDisponibilidadAsync(Disponibilidad disponibilidad);
        Task<bool> EliminarDisponibilidadAsync(int id);
        Task<IEnumerable<Disponibilidad>> ObtenerPorProfesionalAsync(int profesionalId);
        Task<bool> ReservarTurnoAsync(int disponibilidadId, int pacienteId);
        Task<bool> CancelarTurnoAsync(int disponibilidadId);
    }
}