using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestById;

public record GetRecordRequestByIdQuery(Guid Id) : IRequest<RecordRequestModel?>;

internal sealed class GetRecordRequestByIdQueryHandler(IReadRepository repository) : IRequestHandler<GetRecordRequestByIdQuery, RecordRequestModel?>
{
    private readonly IReadRepository _repository = repository;

    public async Task<RecordRequestModel?> Handle(GetRecordRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.RecordRequestsView            
            .Include(r => r.Files)
            .Include(r => r.RequestedRecords)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        return RecordRequestMappers.MapToModel(entity);
    }
}