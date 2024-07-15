using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRequestedRecordIsAvailable;

public record class UpdateRequestedRecordIsAvailableRequest(Guid Id, bool IsActive) : IRequest<Guid?>;

public class UpdateRequestedRecordIsAvailable : IRequestHandler<UpdateRequestedRecordIsAvailableRequest, Guid?>
{
    private readonly IWriteRepository _writeRepository;

    public UpdateRequestedRecordIsAvailable(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid?> Handle(UpdateRequestedRecordIsAvailableRequest request, CancellationToken cancellationToken)
    {
        var requestedRecord = _writeRepository.RequestedRecords.FirstOrDefault(x => x.Id == request.Id);

        if (requestedRecord is null) return null;

        requestedRecord.UpdateIsAvailable(request.IsActive);
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return requestedRecord.Id;
    }
}
