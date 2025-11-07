using school.Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole? Role { get; set; }  
    public string FullName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    public Student()
    {
        
    }
    public Student(int id, string username, string password, string email, string fullName, string grade)
    {
        Id = id;
        Username = username;
        PasswordHash = password;
        Email = email;
        FullName = fullName;
        Grade = grade;
        EnrollmentDate = DateTime.Now;
        Role = UserRole.Student;
    }
}