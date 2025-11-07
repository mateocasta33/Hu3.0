using school.Application.Dtos;
using school.Domain.Entities;

namespace school.Application.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
