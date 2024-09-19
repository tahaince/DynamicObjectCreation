using Micromarin.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Micromarin.Services.Infastructures.Interceptors;

internal class SavingChangesInterceptor : SaveChangesInterceptor {
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = new()) {
        var entries = eventData.Context?.ChangeTracker.Entries()
            .Where(e => e.Entity is DynamicEntity
                || e.State == EntityState.Modified
                || e.State == EntityState.Added);

        foreach (var e in entries ?? []) {
            switch (e.State) {
                case EntityState.Added:
                    e.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    e.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    e.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
