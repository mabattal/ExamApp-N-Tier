using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ExamApp.Repositories.Interceptors
{
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
        {
            { EntityState.Added, AddBehavior },
            { EntityState.Modified, ModifiedBehavior }
        };

        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.CreatedDate = DateTimeOffset.UtcNow;
            context.Entry(auditEntity).Property(x => x.UpdatedDate).IsModified = false;
        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.UpdatedDate = DateTimeOffset.UtcNow;
            context.Entry(auditEntity).Property(x => x.CreatedDate).IsModified = false;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {
                if (entityEntry.Entity is not IAuditEntity auditEntity)
                {
                    continue;
                }

                if (Behaviors.TryGetValue(entityEntry.State, out var behavior))
                {
                    behavior(eventData.Context, auditEntity);
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
