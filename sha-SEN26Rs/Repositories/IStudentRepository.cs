using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student?> GetByUsernameAsync(string username);
    Task<Student?> GetByEmailAsync(string email);
    Task<List<Student>> GetAllAsync();
    Task<List<Student>> SearchAsync(string query);
    Task<Student> CreateAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}
