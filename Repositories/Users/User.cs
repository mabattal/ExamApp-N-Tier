using ExamApp.Repositories.Enums;

namespace ExamApp.Repositories.Users
{
    public class User : IAuditEntity
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
