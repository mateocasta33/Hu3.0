using Moq;
using school.Application.Services;
using school.Domain.Entities;
using school.Domain.Interfaces;
using school.Infrastructure.Repositories;


namespace school.Test.Unit;

public class StudentServicesTest
{
    private readonly Mock<IStudentRepository> _repository;
    private readonly StudentService _studentService;

    public StudentServicesTest()
    {
        _repository = new Mock<IStudentRepository>();
        _studentService = new StudentService(_repository.Object);
    }

    [Fact]
    public async Task ReturnStudent_When_Create()
    {
        var student = new Student(1,
            "juanperez",
            "123456",
            "juan@example.com",
            "Juan Pérez",
            "3º ESO");

        _repository
            .Setup(s => s.Create(student))
            .ReturnsAsync(student);

        var result = await _studentService.Create(student);

        Assert.NotNull(result);
        Assert.Equal("juanperez", student.Username);
        Assert.Equal("123456", student.PasswordHash);
        Assert.Equal("juan@example.com", student.Email);
        Assert.Equal("Juan Pérez", student.FullName);
        Assert.Equal("3º ESO", student.Grade);
    }

   
}