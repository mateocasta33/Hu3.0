using school.Domain.Entities;

namespace school.Application.Interfaces;

public interface ITeacherService
{
    Task<IEnumerable<Teacher>> GetAll();
    Task<Teacher?> GetById(int id);
    Task Create(Teacher teacher);
    Task Update(Teacher teacher);
    Task Delete(int id);
}