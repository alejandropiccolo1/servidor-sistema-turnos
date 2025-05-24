// Services/IUserService.cs
using reservabackend.Models;
using ReservasBackend.Dtos;
using System.Threading.Tasks;

namespace ReservasBackend.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegistroDto dto);
        Task<List<Usuario>> GetAllAsync();
        // Si luego querés agregar login, lo podés poner acá, por ejemplo:
        // Task<(bool Success, string Token)> LoginAsync(LoginDto dto);
    }
}
