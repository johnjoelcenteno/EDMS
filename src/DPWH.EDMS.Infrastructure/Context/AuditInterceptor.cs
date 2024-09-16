using System.Security.Claims;
using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DPWH.EDMS.Infrastructure.Context;

internal sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ClaimsPrincipal _principal;

    public AuditInterceptor(ClaimsPrincipal principal)
    {
        _principal = principal;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not null)
        {
            LogChanges(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void LogChanges(DbContext dbContext)
    {
        var auditables = dbContext.ChangeTracker.Entries<EntityBase>();

        var logs = auditables
            .Where(e => e.State is not (EntityState.Detached or EntityState.Unchanged))
            .Select(e => ChangeLog.Create(
                    GetEntityId(e),
                    e.Entity.GetType().Name,   
                    GetPropertyName(e),
                    GetControlNumber(e),
                    e.State.ToString(),
                    _principal.GetUserId().ToString(),
                    _principal.GetUserName(),
                    _principal.GetFirstName(),
                    _principal.GetLastName(),
                    _principal.GetMiddleInitial(),
                    _principal.GetEmployeeNumber(),
                    BuildChangeLog(e)))
            .ToList();

        dbContext.AddRange(logs);
    }

    private static string GetChangeValueByState(EntityState state, object? value, bool forOldValue = true)
    {
        if (value is null)
            return "";

        return state switch
        {
            EntityState.Added => forOldValue ? "" : value.ToString() ?? "",
            EntityState.Deleted => forOldValue ? value.ToString() ?? "" : "",
            EntityState.Modified => value.ToString() ?? "",
            _ => ""
        };
    }

    private static string GetEntityId(EntityEntry<EntityBase> entry) => entry.State switch
    {
        EntityState.Added => entry.CurrentValues["Id"]?.ToString() ?? "",
        _ => entry.Entity is GeoLocation address ? address.MyId : entry.Entity.Id.ToString()
    };

    private static bool IsEqual(MemberEntry prop, string original, string current) => prop.CurrentValue switch
    {
        decimal => decimal.Compare(original.SafeDecimalParse(), current.SafeDecimalParse()) == decimal.Zero,
        _ => original == current
    };

    private static IEnumerable<ChangeLogItem> BuildChangeLog(EntityEntry<EntityBase> entry)
    {
        return entry.Properties
            .Where(p => !p.Metadata.IsPrimaryKey())
            .Select(p =>
            {
                var original = GetChangeValueByState(entry.State, p.OriginalValue);
                var current = GetChangeValueByState(entry.State, p.CurrentValue, false);

                if (IsEqual(p, original, current))
                {
                    return ChangeLogItem.Create(null, null, null);
                }

                return ChangeLogItem.Create(p.Metadata.Name, original, current);
            })
            .Where(i => i.Field is not null)
            .ToList();
    }

    private static string? GetPropertyName(EntityEntry<EntityBase> entry)
    {
        if (entry.Entity.GetType().Name != nameof(RecordType))
        {
            return null;
        }

        var propertyName = entry.Properties
            .First(p => p.Metadata.Name == nameof(RecordType.Category))
            .CurrentValue;

        return propertyName?.ToString();
    }

    private static string? GetControlNumber(EntityEntry<EntityBase> entry)
    {
        if (entry.Entity.GetType().Name != nameof(RecordRequest))
        {
            return null;
        }

        var propertyName = entry.Properties
            .First(p => p.Metadata.Name == nameof(RecordRequest.ControlNumber))
            .CurrentValue;

        return propertyName?.ToString();
    }
}