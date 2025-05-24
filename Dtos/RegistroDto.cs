namespace ReservasBackend.Dtos
{
    public class RegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!; 
        public string Email { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Rol { get; set; } = "paciente"; //valor por defectos
        
    }
}