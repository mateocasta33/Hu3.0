using school.Application.Interfaces;
using school.Domain.Entities;
using school.Domain.Interfaces;

namespace school.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return await _userRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error en el servicio al obtener todos los usuarios.", ex);
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"No se encontró ningún usuario con ID {id}.");

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en el servicio al obtener el usuario con ID {id}.", ex);
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception("Error en el servicio al crear un usuario.", ex);
        }
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        try
        {
            var existing = await _userRepository.GetByIdAsync(user.Id);
            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {user.Id}.");

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.PasswordHash = user.PasswordHash;
            existing.Role = user.Role;

            await _userRepository.UpdateAsync(existing);
            await _userRepository.SaveChangesAsync();

            return existing;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en el servicio al actualizar el usuario con ID {user.Id}.", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}.");

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en el servicio al eliminar el usuario con ID {id}.", ex);
        }
    }
}

