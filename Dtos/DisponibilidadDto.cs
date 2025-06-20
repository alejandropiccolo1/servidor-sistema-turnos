using reservabackend.Models;

namespace ReservasBackend.Dtos
{
    public class DisponibilidadDto
    {
        public int Id { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int Duracion { get; set; }
        public string Estado { get; set; } = null!;
        public string NombreProfesional { get; set; } = null!;
        public string ApellidoProfesional { get; set; } = null;
        public string Especialidad { get; set; } = null;
    }
}