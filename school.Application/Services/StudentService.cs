using school.Application.Interfaces;
using school.Domain.Entities;
using school.Domain.Interfaces;

namespace school.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Student?> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Student> Create(Student student)
        {
            var existing = await _repository.GetAll();
            if (existing.Any(s => s.Email == student.Email))
                throw new Exception("El correo ya está registrado para otro estudiante");

            return await _repository.Create(student);
        }

        public async Task<Student> Update(Student student)
        {
            var existing = await _repository.GetById(student.Id);
            if (existing == null)
                throw new Exception("El estudiante no existe");

            return await _repository.Update(student);
        }

        public async Task Delete(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null)
                throw new Exception("El estudiante no existe");

            await _repository.Delete(existing);  // ← Pasar la entidad, no el ID
        }
        
    }
}