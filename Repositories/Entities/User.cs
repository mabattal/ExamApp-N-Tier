using ExamApp.Repositories.Enums;

namespace ExamApp.Repositories.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft delete için bayrak
    }
}
