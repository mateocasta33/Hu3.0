using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school.Application.Interfaces;
using school.Domain.Entities;

namespace school.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al obtener los usuarios: {ex.Message}" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al obtener el usuario: {ex.Message}" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (user == null)
            return BadRequest(new { message = "Datos de usuario inválidos" });

        try
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al crear el usuario: {ex.Message}" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (user == null || id != user.Id)
            return BadRequest(new { message = "Datos inválidos o ID no coincide" });

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(user);

            if (updatedUser == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al actualizar el usuario: {ex.Message}" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound(new { message = "Usuario no encontrado o no se pudo eliminar" });

            return Ok(new { message = "Usuario eliminado correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al eliminar el usuario: {ex.Message}" });
        }
    }
}