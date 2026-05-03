namespace sha_SEN26Rs.Models;

public class Specialty
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<StudentSpecialty> StudentSpecialties { get; set; } = [];
}
