
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using school.Application.Dtos;
using school.Domain.Entities;
using school.Domain.Interfaces;

namespace school.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "El objeto de registro no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(dto.Username));

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("El correo electrónico es obligatorio.", nameof(dto.Email));

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(dto.Password));

            if (string.IsNullOrWhiteSpace(dto.Role))
                throw new ArgumentException("El rol es obligatorio.", nameof(dto.Role));

            if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(dto.Email));

            if (dto.Password.Length < 8 ||
                !dto.Password.Any(char.IsUpper) ||
                !dto.Password.Any(char.IsLower) ||
                !dto.Password.Any(char.IsDigit) ||
                !dto.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un símbolo.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Ya existe un usuario con este correo electrónico.");

            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new ArgumentException("El rol especificado no es válido.", nameof(dto.Role));

            var user = new User
            {
                Username = dto.Username.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = role
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            
            return user;
        }


        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            Console.WriteLine($"[DEBUG] Login email: {dto.Email}");
            Console.WriteLine($"[DEBUG] Found user: {(user != null ? user.Email : "NULL")}");
            Console.WriteLine($"[DEBUG] Stored hash: {user?.PasswordHash}");

            if (user == null)
            {
                throw new Exception($"User not found with email: {dto.Email}");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            Console.WriteLine($"[DEBUG] Password valid: {isPasswordValid}");

            if (!isPasswordValid)
            {
                throw new Exception("Password mismatch.");
            }

            return GenerateToken(user);
        }

        
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
