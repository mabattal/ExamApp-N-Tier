namespace ExamApp.Repositories
{
    public interface IAuditEntity
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
