using Microsoft.EntityFrameworkCore;
using reservabackend.Models;

namespace ReservasBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Disponibilidad> Disponibilidades { get; set; } = null!;

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

     modelBuilder.Entity<Disponibilidad>()
        .HasOne(d => d.Profesional)
        .WithMany(u => u.DisponibilidadesComoProfesional)
        .HasForeignKey(d => d.ProfesionalId)
        .OnDelete(DeleteBehavior.Restrict);

     modelBuilder.Entity<Disponibilidad>()
        .HasOne(d => d.Paciente)
        .WithMany(u => u.DisponibilidadesComoPaciente)
        .HasForeignKey(d => d.PacienteId)
        .OnDelete(DeleteBehavior.SetNull);
        }

    }
    
}
