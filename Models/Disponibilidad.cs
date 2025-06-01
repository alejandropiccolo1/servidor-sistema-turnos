
namespace reservabackend.Models
{
    public class Disponibilidad
    {
        public int Id { get; set; }  // Identificador único (importante para BD)

        public DateTime FechaHoraInicio { get; set; }  // Fecha y hora de inicio combinadas

        public DateTime FechaHoraFin { get; set; }  // Fecha y hora fin calculada

        public int Duracion { get; set; }  // Duración en minutos (ej: 30)

        public int ProfesionalId { get; set; }  // FK al profesional que creó el turno (si aplica)

        // Podrías agregar estado, por ejemplo: Disponible, Reservado, Cancelado
        public string Estado { get; set; } = "Disponible";
        public int? PacienteId { get; set; }
    }


}