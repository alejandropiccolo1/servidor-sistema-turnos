// Repositories/IUserRepository.cs
using reservabackend.Models;
using System.Threading.Tasks;

namespace ReservasBackend.Repositories
{
    public interface IUserRepository
    {
        Task<bool>? EmailExistsAsync(string email); //si existe el mail
        Task? AddUserAsync(Usuario usuario); //agrega usuario    
        //  Task SaveChangesAsync(); //guarda cambios   
        Task<List<Usuario>> GetAllUsersAsync(); //devuelve lista de usuarios
        Task SaveChangesAsync();

        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario> GetById(int userId);
    }
}
