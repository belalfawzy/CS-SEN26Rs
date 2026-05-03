using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Data;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public async Task<Student?> GetByIdAsync(Guid id) =>
        await context.Students
            .Include(s => s.Team)
            .Include(s => s.StudentSpecialties).ThenInclude(ss => ss.Specialty)
            .Include(s => s.SocialLinks)
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Student?> GetByUsernameAsync(string username) =>
        await context.Students
            .Include(s => s.Team)
            .Include(s => s.StudentSpecialties).ThenInclude(ss => ss.Specialty)
            .Include(s => s.SocialLinks)
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Username == username);

    public async Task<Student?> GetByEmailAsync(string email) =>
        await context.Students.FirstOrDefaultAsync(s => s.Email == email);

    public async Task<List<Student>> GetAllAsync() =>
        await context.Students
            .Include(s => s.Team)
            .Include(s => s.StudentSpecialties).ThenInclude(ss => ss.Specialty)
            .Include(s => s.SocialLinks)
            .OrderBy(s => s.FullName)
            .ToListAsync();

    public async Task<Student> CreateAsync(Student student)
    {
        student.Id = Guid.NewGuid();
        student.CreatedAt = DateTime.UtcNow;
        student.UpdatedAt = DateTime.UtcNow;
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        student.UpdatedAt = DateTime.UtcNow;
        context.Students.Update(student);
        await context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> UsernameExistsAsync(string username) =>
        await context.Students.AnyAsync(s => s.Username == username);

    public async Task<bool> EmailExistsAsync(string email) =>
        await context.Students.AnyAsync(s => s.Email == email);
}
