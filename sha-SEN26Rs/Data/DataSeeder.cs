using sha_SEN26Rs.Models;

namespace sha_SEN26Rs.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Teams.Any()) return;

        var password = BCrypt.Net.BCrypt.HashPassword("ChangeMe@123");

        var teams = new List<Team>
        {
            new() { Id = Guid.Parse("53cf5446-9de5-4127-91cd-7de50278dd68"), TeamNumber = 8,  Name = "Team 8",  CreatedAt = new DateTime(2026, 5, 2, 16, 59, 32, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("f4782d6e-6fec-422f-9eba-6571cd10d8d6"), TeamNumber = 22, Name = "Team 22", CreatedAt = new DateTime(2026, 5, 2, 16, 59, 40, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("29ca7f35-e6b1-4506-8802-3ed8c6236747"), TeamNumber = 18, Name = "Team 18", CreatedAt = new DateTime(2026, 5, 2, 17,  5, 31, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("25a76239-4df4-45ab-97c4-4598b86ecf52"), TeamNumber = 21, Name = "Team 21", CreatedAt = new DateTime(2026, 5, 2, 18, 47, 34, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("6464db14-8706-4edd-94e9-a24af84b2245"), TeamNumber = 17, Name = "Team 17", CreatedAt = new DateTime(2026, 5, 2, 19,  4, 31, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("83cea836-2b89-47cb-9e8b-9c506a3ecd35"), TeamNumber = 40, Name = "Team 40", CreatedAt = new DateTime(2026, 5, 2, 19,  6,  5, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("3b067816-5cf6-41c4-8c90-37c38918e94a"), TeamNumber = 1,  Name = "Team 1",  CreatedAt = new DateTime(2026, 5, 2, 21, 20,  4, DateTimeKind.Utc) },
        };
        db.Teams.AddRange(teams);

        var students = new List<Student>
        {
            new() {
                Id = Guid.Parse("370536d1-8c91-4d4d-afbf-0a204d0b95bc"),
                Email = "marvelmagde@gmail.com",
                PasswordHash = password,
                Username = "marvola",
                FullName = "Marvel",
                Nickname = "Marvola",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/124ea697-4440-4530-80c4-5fff1fa51da7/124ea697-4440-4530-80c4-5fff1fa51da7.png",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Mobile app developer",
                IsOnboarded = true,
                TeamId = Guid.Parse("f4782d6e-6fec-422f-9eba-6571cd10d8d6"),
                Phone = "01272010558",
                Location = "Cairo",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 16, 57, 45, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 20,  4, 53, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("0f9ada07-99ed-4bb5-af5e-efc0cb1b3563"),
                Email = "nourelden.dev@gmail.com",
                PasswordHash = password,
                Username = "nourelden",
                FullName = "New User",
                IsOnboarded = false,
                PrivacySetting = "public",
                GraduationYear = 2026,
                CreatedAt = new DateTime(2026, 5, 2, 18, 46,  2, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 18, 46,  2, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("78276c53-eb31-43c4-81e6-b1efea44ef3c"),
                Email = "rehamsaeed833@gmail.com",
                PasswordHash = password,
                Username = "rahom",
                FullName = "Reham Saeed",
                Nickname = "Rahom",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/f5de451f-29da-4ffa-8df8-fee16a829757/f5de451f-29da-4ffa-8df8-fee16a829757.png",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Frontend developer",
                IsOnboarded = true,
                TeamId = Guid.Parse("25a76239-4df4-45ab-97c4-4598b86ecf52"),
                Phone = "01000183566",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 18, 43, 41, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 18, 47, 34, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("9cd9e5ce-6829-47bf-bc6e-ea5d83cea0a2"),
                Email = "habebahesham97@gmail.com",
                PasswordHash = password,
                Username = "bebo",
                FullName = "habeba hesham",
                Nickname = "bebo",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/966cd93b-ce70-4091-9b98-d06d9094218f/966cd93b-ce70-4091-9b98-d06d9094218f.png",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "ai developer",
                IsOnboarded = true,
                TeamId = Guid.Parse("25a76239-4df4-45ab-97c4-4598b86ecf52"),
                Phone = "01019502217",
                Location = "el obour",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 18, 43, 50, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 18, 48,  3, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("503bea4b-1b1d-4f56-bdef-94b4b34e8827"),
                Email = "roserere47@gmail.com",
                PasswordHash = password,
                Username = "fattoma",
                FullName = "Fatima Elhady Mohammed",
                Nickname = "fattoma",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/128b579b-8d06-48e6-83fe-4fe77c18b172/128b579b-8d06-48e6-83fe-4fe77c18b172.jpg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Front-End",
                IsOnboarded = true,
                TeamId = Guid.Parse("29ca7f35-e6b1-4506-8802-3ed8c6236747"),
                Phone = "01017401891",
                Location = "El sharqia",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 17, 45,  7, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 18, 59, 56, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("827637d4-795c-4b54-b89d-a0ab9896139c"),
                Email = "janasaffour113@gmail.com",
                PasswordHash = password,
                Username = "janasaffour",
                FullName = "New User",
                IsOnboarded = false,
                PrivacySetting = "public",
                GraduationYear = 2026,
                CreatedAt = new DateTime(2026, 5, 2, 19,  0,  1, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19,  0,  1, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("358169db-91c8-4b14-b375-e61e36f3740b"),
                Email = "belalfawzy123@gmail.com",
                PasswordHash = password,
                Username = "belaaal",
                FullName = "Belal Fawzy",
                Nickname = "Belaaal",
                Bio = "Belaaaal",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/847919f5-be27-448b-a6bc-dca97da20fe7/847919f5-be27-448b-a6bc-dca97da20fe7.jpg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "CS",
                IsOnboarded = true,
                TeamId = Guid.Parse("53cf5446-9de5-4127-91cd-7de50278dd68"),
                Phone = "01124259475",
                Location = "Cairo, Egypt",
                Website = "https://belal-fawzy.vercel.app/",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 18, 58, 28, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19,  0, 39, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("2c184444-93c2-41aa-99bc-a3210cc84ca2"),
                Email = "323230216@sha.edu.eg",
                PasswordHash = password,
                Username = "demoo",
                FullName = "Demiana Adel",
                Nickname = "Demoo",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/c7c63ad4-6c50-4ed0-a077-dd82f9745daf/c7c63ad4-6c50-4ed0-a077-dd82f9745daf.jpeg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "UIUX",
                IsOnboarded = true,
                TeamId = Guid.Parse("25a76239-4df4-45ab-97c4-4598b86ecf52"),
                Phone = "01274060247",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 18, 56,  2, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 20, 25, 38, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("6cfc877f-5285-431f-b722-d7ff0be5817d"),
                Email = "georgesabry35@gmail.com",
                PasswordHash = password,
                Username = "georgesabry",
                FullName = "George",
                Nickname = "جوج",
                Bio = "الجوج",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/3e0f37f0-b44e-49bb-9825-0f6ba7a12616/3e0f37f0-b44e-49bb-9825-0f6ba7a12616.jpg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Mobile app",
                IsOnboarded = true,
                TeamId = Guid.Parse("3b067816-5cf6-41c4-8c90-37c38918e94a"),
                Phone = "+20 15 55477241",
                Location = "Cairo",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 16, 58,  2, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 21, 20,  4, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("7259fdae-1240-400f-9851-6441a5ccbdce"),
                Email = "rokiamagdy54@gmail.com",
                PasswordHash = password,
                Username = "reka",
                FullName = "Rokia magdy",
                Nickname = "Reka",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/e8ffa77e-8b0d-412c-88f4-98d3b8aba5a4/e8ffa77e-8b0d-412c-88f4-98d3b8aba5a4.jpg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Back end developer",
                IsOnboarded = true,
                TeamId = Guid.Parse("6464db14-8706-4edd-94e9-a24af84b2245"),
                Phone = "+20 102 358 798 1",
                Location = "Egypt",
                PrivacySetting = "private",
                CreatedAt = new DateTime(2026, 5, 2, 16, 58, 50, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19, 19, 20, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("cafd863a-4f75-4152-9bef-c2f001cf9b59"),
                Email = "radwaaboelyazead@gmail.com",
                PasswordHash = password,
                Username = "rodeez",
                FullName = "Radwa Aboelyazead",
                Nickname = "Rodeez",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/7a0f22f9-087c-43d7-a39b-c44849d8fbb6/7a0f22f9-087c-43d7-a39b-c44849d8fbb6.jpeg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Cs",
                IsOnboarded = true,
                TeamId = Guid.Parse("f4782d6e-6fec-422f-9eba-6571cd10d8d6"),
                Phone = "1030967385",
                Location = "Sha",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 19,  7,  8, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19,  8, 53, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("5440dc31-11c1-46c1-8e20-12259266001e"),
                Email = "fadyemad933@gmail.com",
                PasswordHash = password,
                Username = "forzzz",
                FullName = "Fady Emad",
                Nickname = "Forzzz",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/c7b6b86d-8b8b-4a32-9a53-82494963a3e6/c7b6b86d-8b8b-4a32-9a53-82494963a3e6.jpeg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Front-End",
                IsOnboarded = true,
                TeamId = Guid.Parse("29ca7f35-e6b1-4506-8802-3ed8c6236747"),
                Phone = "01203289612",
                Location = "cairo",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 17, 18,  1, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19, 25, 25, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("d3670767-2f88-4f31-975a-07e1b37ee6cd"),
                Email = "rawannnmagdy221@gmail.com",
                PasswordHash = password,
                Username = "rawanmagdy",
                FullName = "New User",
                IsOnboarded = false,
                PrivacySetting = "public",
                GraduationYear = 2026,
                CreatedAt = new DateTime(2026, 5, 2, 19, 34,  8, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19, 34,  8, DateTimeKind.Utc)
            },
            new() {
                Id = Guid.Parse("285c1195-768d-4f95-aea6-d64386e18784"),
                Email = "mohamedzahrann0@gmail.com",
                PasswordHash = password,
                Username = "zahran",
                FullName = "Mohamed Osama Zahran",
                Nickname = "Zahran",
                AvatarUrl = "https://bgdfzbqmhwcnfmcbthlb.supabase.co/storage/v1/object/public/sha-gallery/avatars/26e782be-b9b9-4c88-ba01-8035cc83a93a/26e782be-b9b9-4c88-ba01-8035cc83a93a.jpeg",
                GraduationYear = 2026,
                GraduationProjectSpecialty = "Frontend&Backend&AI",
                IsOnboarded = true,
                TeamId = Guid.Parse("83cea836-2b89-47cb-9e8b-9c506a3ecd35"),
                Phone = "+201092088922",
                Location = "Cairo",
                Website = "https://mohamed-zahrann.vercel.app/",
                PrivacySetting = "public",
                CreatedAt = new DateTime(2026, 5, 2, 19,  3, 59, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 2, 19, 12,  2, DateTimeKind.Utc)
            },
        };
        db.Students.AddRange(students);

        var socialLinks = new List<SocialLink>
        {
            new() { StudentId = Guid.Parse("358169db-91c8-4b14-b375-e61e36f3740b"), Platform = "GitHub",   Url = "https://github.com/belalfawzy",                         CreatedAt = DateTime.UtcNow },
            new() { StudentId = Guid.Parse("358169db-91c8-4b14-b375-e61e36f3740b"), Platform = "LinkedIn", Url = "https://www.linkedin.com/in/belalfawzy123",              CreatedAt = DateTime.UtcNow },
            new() { StudentId = Guid.Parse("2c184444-93c2-41aa-99bc-a3210cc84ca2"), Platform = "LinkedIn", Url = "https://www.linkedin.com/in/demiana-adel-209642269",     CreatedAt = DateTime.UtcNow },
            new() { StudentId = Guid.Parse("285c1195-768d-4f95-aea6-d64386e18784"), Platform = "GitHub",   Url = "https://github.com/Zahrannnn",                          CreatedAt = DateTime.UtcNow },
            new() { StudentId = Guid.Parse("285c1195-768d-4f95-aea6-d64386e18784"), Platform = "LinkedIn", Url = "https://www.linkedin.com/in/mohamed-zahran-383859222/",  CreatedAt = DateTime.UtcNow },
        };
        db.SocialLinks.AddRange(socialLinks);

        await db.SaveChangesAsync();
    }
}
