using school.Application.Interfaces;
using school.Domain.Entities;
using school.Domain.Interfaces;

namespace school.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }


    public async Task<IEnumerable<Teacher>> GetAll()
    {
        return await _teacherRepository.GetAll();
    }

    public async Task<Teacher?> GetById(int id)
    {
        return await _teacherRepository.GetById(id);
    }

    public async Task Create(Teacher teacher)
    {
        var existingTeachers = await _teacherRepository.GetAll();

        if (existingTeachers.Any(t => t.Username.Equals(teacher.Username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ya existe un docente con ese nombre de usuario.");

        if (existingTeachers.Any(t => t.Email.Equals(teacher.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ya existe un docente con ese correo electrónico.");

        // Si todo está correcto, crear el docente
        await _teacherRepository.Create(teacher);
}

    public async Task Update(Teacher teacher)
    {
        var existingTeacher = await _teacherRepository.GetById(teacher.Id);

        if (existingTeacher is null)
            throw new KeyNotFoundException("El docente no existe.");

        var allTeachers = await _teacherRepository.GetAll();

        // Validar duplicados (excepto el propio)
        if (allTeachers.Any(t => t.Username.Equals(teacher.Username, StringComparison.OrdinalIgnoreCase) && t.Id != teacher.Id))
            throw new InvalidOperationException("Ya existe otro docente con ese nombre de usuario.");

        if (allTeachers.Any(t => t.Email.Equals(teacher.Email, StringComparison.OrdinalIgnoreCase) && t.Id != teacher.Id))
            throw new InvalidOperationException("Ya existe otro docente con ese correo electrónico.");

        // Actualizar datos permitidos
        existingTeacher.Username = teacher.Username;
        existingTeacher.Email = teacher.Email;
        existingTeacher.TeacherName = teacher.TeacherName;
        existingTeacher.Specialty = teacher.Specialty;
        existingTeacher.PasswordHash = teacher.PasswordHash;

        await _teacherRepository.Update(existingTeacher);
    }

    public async Task Delete(int id)
    {
        var teacher = await _teacherRepository.GetById(id);

        if (teacher is null)
            throw new KeyNotFoundException("El docente no existe.");

        await _teacherRepository.Delete(id);
    }
}