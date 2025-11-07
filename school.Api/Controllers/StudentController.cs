using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school.Application.Interfaces;
using school.Domain.Entities;

namespace school.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    { 
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _service.GetAll();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _service.GetById(id);
        if (student == null)
            return NotFound(new { message = "Estudiante no encontrado" });

        return Ok(student);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        try
        {
            await _service.Create(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Student student)
    {
        if (id != student.Id)
            return BadRequest(new { message = "El ID no coincide" });

        try
        {
            await _service.Update(student);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message }); 
                
        }
    }
}
