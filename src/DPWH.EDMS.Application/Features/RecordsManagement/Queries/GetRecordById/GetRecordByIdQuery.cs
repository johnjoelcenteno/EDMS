using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordsManagement.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordById;

public record GetRecordByIdQuery(Guid Id) : IRequest<RecordModel?>;

internal sealed class GetRecordByIdQueryHandler(IReadRepository repository) : IRequestHandler<GetRecordByIdQuery, RecordModel?>
{
    private readonly IReadRepository _repository = repository;

    public async Task<RecordModel?> Handle(GetRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.RecordsView
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        return RecordMappers.MapToModel(entity);
    }
}