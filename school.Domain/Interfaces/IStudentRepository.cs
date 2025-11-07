using school.Domain.Entities;

namespace school.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAll();
        Task<Student?> GetById(int id);
        Task<Student> Create(Student student);
        Task<Student> Update(Student student);
        Task Delete(Student student);
    }
}