namespace sha_SEN26Rs.Models;

public class StudentSpecialty
{
    public long Id { get; set; }
    public Guid StudentId { get; set; }
    public long SpecialtyId { get; set; }

    public Student Student { get; set; } = null!;
    public Specialty Specialty { get; set; } = null!;
}
