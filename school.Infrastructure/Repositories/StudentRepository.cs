using Microsoft.EntityFrameworkCore;
using school.Domain.Entities;
using school.Domain.Interfaces;
using school.Infrastructure.Data;

namespace school.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }
        public async Task<Student?> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> Create(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> Update(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
    
            if (existingStudent == null)
                throw new Exception("Estudiante no encontrado");

            existingStudent.FullName = student.FullName;
            existingStudent.Grade = student.Grade;
            existingStudent.EnrollmentDate = student.EnrollmentDate;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task Delete(Student student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}