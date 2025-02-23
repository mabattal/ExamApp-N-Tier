using ExamApp.Repositories.Enums;

namespace ExamApp.Repositories.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
