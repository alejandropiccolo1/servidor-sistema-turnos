// Services/IUserService.cs
using reservabackend.Models;
using ReservasBackend.Dtos;
using System.Threading.Tasks;

namespace ReservasBackend.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegistroDto dto); //registro
        Task<List<Usuario>> GetAllAsync(); // obtener usuarios 
        Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto dto);
    }
}
