namespace reservabackend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Rol { get; set; } = "paciente";
        public string Especialidad { get; set; } = null!;
       public ICollection<Disponibilidad> ?DisponibilidadesComoProfesional { get; set; }
        public ICollection<Disponibilidad> ?DisponibilidadesComoPaciente { get; set; }
    }
}