using TutoringPlatform.Application.Mappings;
using TutoringPlatform.Domain.Entities;
using TutoringPlatform.Domain.Enums;

namespace TutoringPlatform.Tests;

public class MappingExtensionsTests
{
    [Fact]
    public void ToDetailDto_ShouldMapTutorSubjectsAlphabetically()
    {
        var tutor = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com",
            Role = UserRole.Tutor,
            TutorProfile = new TutorProfile
            {
                Headline = "STEM specialist",
                Bio = "Experienced tutor",
                HourlyRate = 50,
                YearsOfExperience = 6,
                TutorSubjects =
                [
                    new TutorSubject { SubjectId = Guid.NewGuid(), Subject = new Subject { Name = "Physics" } },
                    new TutorSubject { SubjectId = Guid.NewGuid(), Subject = new Subject { Name = "Algebra" } }
                ]
            }
        };

        var dto = tutor.ToDetailDto();

        Assert.Equal(["Algebra", "Physics"], dto.Subjects.Select(x => x.Name).ToArray());
        Assert.Equal("Ada Lovelace", dto.FullName);
    }
}
