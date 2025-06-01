using ReservasBackend.Data;
using reservabackend.Models;
using Microsoft.EntityFrameworkCore;


namespace ReservasBackend.Repositories
{
    public class DisponibilidadRepository : IDisponibilidadRepository
    {
        private readonly ApplicationDbContext _context;
        public DisponibilidadRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task<IEnumerable<Disponibilidad>> ObtenerTodasAsync()
        {
            return await _context.Disponibilidades.ToListAsync();
        }

        public async Task<Disponibilidad> ObtenerPorIdAsync(int id)
        {
            return await _context.Disponibilidades.FindAsync(id);
        }

        public async Task AgregarAsync(Disponibilidad disponibilidad)
        {
            await _context.Disponibilidades.AddAsync(disponibilidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Disponibilidad disponibilidad)
        {
            var disponibilidadExistente = await _context.Disponibilidades.FindAsync(disponibilidad.Id);
        if (disponibilidadExistente == null)
        {
        throw new Exception("Disponibilidad no encontrada");
         }

        // Actualizar solo los campos que pueden cambiar:
        disponibilidadExistente.Duracion = disponibilidad.Duracion;
        disponibilidadExistente.Estado = disponibilidad.Estado;
        disponibilidadExistente.FechaHoraInicio = disponibilidad.FechaHoraInicio;
        disponibilidadExistente.FechaHoraFin = disponibilidad.FechaHoraFin;

        // No modifiques el ProfesionalId ni el Id aquí (salvo que sea estrictamente necesario)

        await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entity = await _context.Disponibilidades.FindAsync(id);
            if (entity != null)
            {
                _context.Disponibilidades.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Disponibilidad>> ObtenerPorProfesionalAsync(int profesionalId)
        {
            return await _context.Disponibilidades
                .Where(d => d.ProfesionalId == profesionalId)
                .ToListAsync();
        }

        public async Task<bool> ExisteSolapamientoAsync(DateTime inicio, DateTime fin, int profesionalId)
        {
            return await _context.Disponibilidades.AnyAsync(d =>
                d.ProfesionalId == profesionalId &&
                d.Estado == "Disponible" &&
                (
                    (inicio >= d.FechaHoraInicio && inicio < d.FechaHoraFin) ||
                    (fin > d.FechaHoraInicio && fin <= d.FechaHoraFin) ||
                    (inicio <= d.FechaHoraInicio && fin >= d.FechaHoraFin)
                )
            );
        }
    }
}