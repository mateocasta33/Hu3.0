namespace school.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
        public string TeacherName { get; set; }
        public string Specialty { get; set; }

        public Teacher()
        {
            
        }
        public Teacher(int id, string username, string password, string email, string teacherName, string specialty)
        {
            Id = id;
            Username = username;
            PasswordHash = password;
            Email = email;
            TeacherName = teacherName;
            Specialty = specialty;
            Role = UserRole.Teacher;
        }
    }
}