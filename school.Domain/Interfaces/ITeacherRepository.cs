using school.Domain.Entities;

namespace school.Domain.Interfaces;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAll();
    Task<Teacher?> GetById(int id);
    Task Create(Teacher user);
    Task Update(Teacher user);
    Task Delete(int id);
}