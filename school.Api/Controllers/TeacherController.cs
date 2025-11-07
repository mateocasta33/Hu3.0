using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school.Application.Interfaces;
using school.Domain.Entities;

namespace school.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAll();
            return Ok(teachers);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetById(id);

            if (teacher == null)
                return NotFound("No se encontró el docente.");

            return Ok(teacher);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest("Los datos del docente son requeridos.");

            await _teacherService.Create(teacher);
            return Ok("Docente creado correctamente.");
        }

 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Teacher teacher)
        {
            if (teacher == null || id != teacher.Id)
                return BadRequest("El ID del docente no coincide o los datos son inválidos.");

            await _teacherService.Update(teacher);
            return Ok("Docente actualizado correctamente.");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.Delete(id);
            return Ok("Docente eliminado correctamente.");
        }
    }
}
