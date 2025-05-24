using ReservasBackend.Repositories;
using ReservasBackend.Dtos;
using reservabackend.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReservasBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegistroDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                return (false, "El email ya está registrado");

            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Contraseña));
            var hashedPassword = Convert.ToBase64String(hashedBytes);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                PasswordHash = hashedPassword,
            };

            await _userRepository.AddUserAsync(usuario);
            await _userRepository.SaveChangesAsync();

            return (true, "Usuario registrado correctamente");
        }
        
            public async Task<List<Usuario>> GetAllAsync()
            {
                return await _userRepository.GetAllUsersAsync();
            }

    }
}
