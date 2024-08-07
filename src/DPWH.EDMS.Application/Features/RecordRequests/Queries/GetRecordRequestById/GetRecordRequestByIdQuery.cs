using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Mappers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestById;

public record GetRecordRequestByIdQuery(Guid Id) : IRequest<RecordRequestModel?>;

internal sealed class GetRecordRequestByIdQueryHandler(IReadRepository repository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<GetRecordRequestByIdQuery, RecordRequestModel?>
{
    private readonly IReadRepository _repository = repository;
    private readonly ClaimsPrincipal _claimsPrincipal = claimsPrincipal;

    public async Task<RecordRequestModel?> Handle(GetRecordRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.RecordRequestsView            
            .Include(r => r.Files)
            .Include(r => r.RequestedRecords)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) return null;        

        var result = RecordRequestMappers.MapToModel(entity);

        //Check if HRMD or RMD
        if (_claimsPrincipal.IsInRole(ApplicationRoles.Staff) || _claimsPrincipal.IsInRole(ApplicationRoles.Manager))
        {
            result.RequestedRecords = result.RequestedRecords.Where(r => r.Office == _claimsPrincipal.GetOffice()).ToList();
        }

        return result;        
    }
}