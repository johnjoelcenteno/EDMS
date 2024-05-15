using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Mappers;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringById;

public record GetProjectMonitoringByIdQuery(Guid Id) : IRequest<GetProjectMonitoringByIdResult>;
internal sealed class GetProjectMonitoringById : IRequestHandler<GetProjectMonitoringByIdQuery, GetProjectMonitoringByIdResult>
{
    private readonly IReadRepository _repository;

    public GetProjectMonitoringById(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetProjectMonitoringByIdResult> Handle(GetProjectMonitoringByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.ProjectMonitoringView
            .Include(x => x.ProjectMonitoringBuildingComponents)
            .Where(x => x.Id == request.Id)
            .Select(ProjectMonitoringMappers.MapToModelExpression())
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new AppException("No project monitoring found");

        return new GetProjectMonitoringByIdResult(entity);
    }
}
