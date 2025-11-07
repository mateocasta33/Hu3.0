using school.Domain.Entities;

namespace school.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAll();
        Task<Student?> GetById(int id);
        Task<Student> Create(Student student);
        Task<Student> Update(Student student);
        Task Delete(int id);
    }
}