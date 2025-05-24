// Repositories/IUserRepository.cs
using reservabackend.Models;
using System.Threading.Tasks;

namespace ReservasBackend.Repositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(Usuario usuario);
        Task SaveChangesAsync();
        Task<List<Usuario>> GetAllUsersAsync();
    }
}
