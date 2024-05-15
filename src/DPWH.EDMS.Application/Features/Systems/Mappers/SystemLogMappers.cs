using DPWH.EDMS.Domain.Entities;
using System.Linq.Expressions;
using DPWH.EDMS.Application.Features.Systems.Queries.Models;

namespace DPWH.EDMS.Application.Features.Systems.Mappers;
public static class SystemLogMappers
{
    public static SystemLogsModel MapToModel(SystemLog entity)
    {
        return new SystemLogsModel
        {
            Id = entity.Id,
            Version = entity.Version,
            Description = entity.Description,
            Created = entity.Created,
            CreatedBy = entity.CreatedBy
        };
    }

    public static Expression<Func<SystemLog, SystemLogsModel>> MapToModelExpression()
    {
        return entity => new SystemLogsModel
        {
            Id = entity.Id,
            Version = entity.Version,
            Description = entity.Description,
            Created = entity.Created,
            CreatedBy = entity.CreatedBy
        };
    }
}
