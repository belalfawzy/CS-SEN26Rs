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

    public async Task<List<Student>> SearchAsync(string query)
    {
        var q = query.Trim();
        if (q.Length == 0) return [];

        var escaped = q.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        var pattern = $"%{escaped}%";

        int? teamNumber = int.TryParse(q, out var tn) ? tn : null;

        return await context.Students
            .Include(s => s.Team)
            .Include(s => s.StudentSpecialties).ThenInclude(ss => ss.Specialty)
            .Include(s => s.SocialLinks)
            .Include(s => s.Images)
            .Where(s =>
                EF.Functions.Like(s.Username, pattern) ||
                EF.Functions.Like(s.FullName, pattern) ||
                (s.Nickname != null && EF.Functions.Like(s.Nickname, pattern)) ||
                (s.Bio != null && EF.Functions.Like(s.Bio, pattern)) ||
                (s.Location != null && EF.Functions.Like(s.Location, pattern)) ||
                (s.Phone != null && EF.Functions.Like(s.Phone, pattern)) ||
                (s.Website != null && EF.Functions.Like(s.Website, pattern)) ||
                (s.GraduationProjectSpecialty != null && EF.Functions.Like(s.GraduationProjectSpecialty, pattern)) ||
                (s.Team != null && EF.Functions.Like(s.Team.Name, pattern)) ||
                (s.Team != null && s.Team.ProjectName != null && EF.Functions.Like(s.Team.ProjectName, pattern)) ||
                s.StudentSpecialties.Any(ss => EF.Functions.Like(ss.Specialty.Name, pattern)) ||
                (teamNumber != null && s.Team != null && s.Team.TeamNumber == teamNumber))
            .OrderBy(s => s.FullName)
            .ToListAsync();
    }

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
