// Repositories/UserRepository.cs
using ReservasBackend.Data;
using reservabackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ReservasBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);

        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetById(int userId) => await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
        
    }
}
