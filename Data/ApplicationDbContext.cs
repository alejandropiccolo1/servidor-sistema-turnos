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
    }
    
}
