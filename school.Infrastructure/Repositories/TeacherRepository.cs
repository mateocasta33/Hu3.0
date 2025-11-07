using Microsoft.EntityFrameworkCore;
using school.Domain.Entities;
using school.Domain.Interfaces;
using school.Infrastructure.Data;

namespace school.Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Teacher>> GetAll()
    {
        return  await _context.Teachers.ToListAsync();
    }

    public async Task<Teacher?> GetById(int id)
    {
        return await _context.Teachers.FindAsync(id);
    }

    public async Task Create(Teacher user)
    {
        await _context.Teachers.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Teacher user)
    {
        _context.Teachers.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}