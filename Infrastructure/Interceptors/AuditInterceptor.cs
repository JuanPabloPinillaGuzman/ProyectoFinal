using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null)
                return result;

            var currentUser = GetCurrentUser();
            LogChanges(context, currentUser);
            return result;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return result;

            var currentUser = GetCurrentUser();
            LogChanges(context, currentUser);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void LogChanges(DbContext context, string currentUser)
        {
            var auditLogs = new List<Auditory>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    auditLogs.Add(new Auditory
                    {
                        EntityName = entry.Entity.GetType().Name,
                        ChangeType = entry.State.ToString(),
                        ChangedBy = currentUser
                    });
                }
            }

            if (auditLogs.Any())
            {
                context.Set<Auditory>().AddRange(auditLogs);
            }
        }

        private string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                // Puedes cambiar "uid" por el claim que uses en tu JWT
                return httpContext.User.FindFirst("uid")?.Value
                    ?? httpContext.User.Identity.Name
                    ?? "SystemUser";
            }
            return "SystemUser";
        }
    }
}