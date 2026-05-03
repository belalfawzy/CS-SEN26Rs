using Microsoft.EntityFrameworkCore;
using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MemoryBoard> MemoryBoards => Set<MemoryBoard>();
    public DbSet<BoardItem> BoardItems => Set<BoardItem>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<StudentSpecialty> StudentSpecialties => Set<StudentSpecialty>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<UserImage> UserImages => Set<UserImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.Username).IsUnique();
            entity.HasIndex(s => s.Email).IsUnique();
            entity.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Username).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Email).IsRequired().HasMaxLength(256);
            entity.Property(s => s.PasswordHash).IsRequired();
            entity.Property(s => s.PrivacySetting).HasDefaultValue("public");
            entity.Property(s => s.GraduationYear).HasDefaultValue(2026);

            entity.HasOne(s => s.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.TeamNumber).IsUnique();
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Content).IsRequired().HasMaxLength(2000);

            entity.HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(m => m.Receiver)
                .WithMany(s => s.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MemoryBoard>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.OwnerStudentId).IsUnique();
            entity.Property(b => b.Width).HasDefaultValue(1200);
            entity.Property(b => b.Height).HasDefaultValue(1600);

            entity.HasOne(b => b.Owner)
                .WithOne(s => s.MemoryBoard)
                .HasForeignKey<MemoryBoard>(b => b.OwnerStudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BoardItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Type).IsRequired().HasMaxLength(50);
            entity.Property(i => i.X).HasPrecision(10, 2);
            entity.Property(i => i.Y).HasPrecision(10, 2);
            entity.Property(i => i.Width).HasPrecision(10, 2);
            entity.Property(i => i.Height).HasPrecision(10, 2);
            entity.Property(i => i.Rotation).HasPrecision(6, 2);

            entity.HasOne(i => i.Board)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Author)
                .WithMany()
                .HasForeignKey(i => i.AuthorStudentId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.Name).IsUnique();
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<StudentSpecialty>(entity =>
        {
            entity.HasKey(ss => ss.Id);
            entity.HasIndex(ss => new { ss.StudentId, ss.SpecialtyId }).IsUnique();

            entity.HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSpecialties)
                .HasForeignKey(ss => ss.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ss => ss.Specialty)
                .WithMany(s => s.StudentSpecialties)
                .HasForeignKey(ss => ss.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SocialLink>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Platform).IsRequired().HasMaxLength(50);
            entity.Property(l => l.Url).IsRequired().HasMaxLength(500);

            entity.HasOne(l => l.Student)
                .WithMany(s => s.SocialLinks)
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserImage>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.PublicId).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Url).IsRequired().HasMaxLength(500);
            entity.Property(i => i.FileName).IsRequired().HasMaxLength(255);

            entity.HasOne(i => i.Student)
                .WithMany(s => s.Images)
                .HasForeignKey(i => i.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
