using ReservasBackend.Repositories;
using ReservasBackend.Dtos;
using reservabackend.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReservasBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["JwtSecret"] ?? "clave-secreta-predeterminada"; // ⚠️ Mejor almacenala en appsettings o como variable de entorno
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
                Rol = dto.Rol,
                Especialidad = dto.Especialidad
            };

            await _userRepository.AddUserAsync(usuario);
            await _userRepository.SaveChangesAsync();

            return (true, "Usuario registrado correctamente");
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<(bool Success, string Message, string Token)> LoginAsync(LoginDto dto)
        {
            var usuario = await _userRepository.GetByEmailAsync(dto.Email);

            if (usuario == null)
                return (false, "Usuario o contraseña incorrectos", null);

            Console.WriteLine($"Especialidad desde BD: {usuario.Especialidad}");    
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Contraseña));
            var hashedPassword = Convert.ToBase64String(hashedBytes);

            if (usuario.PasswordHash != hashedPassword)
                return (false, "Usuario o contraseña incorrectos", null);

            // ✅ Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("apellido", usuario.Apellido ?? ""),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"), // Rol por defecto
                    new Claim("Especialidad", usuario.Especialidad ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (true, "Login exitoso", tokenString);
        }
    }
}
