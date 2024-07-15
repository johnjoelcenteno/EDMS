using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRequestedRecordIsAvailable;

public record class UpdateRequestedRecordIsAvailableRequest(List<Guid> Ids, bool isAvailable) : IRequest<List<Guid>?>;

public class UpdateRequestedRecordIsAvailable : IRequestHandler<UpdateRequestedRecordIsAvailableRequest, List<Guid>?>
{
    private readonly IWriteRepository _writeRepository;

    public UpdateRequestedRecordIsAvailable(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _writeRepository = writeRepository;
    }
    public async Task<List<Guid>?> Handle(UpdateRequestedRecordIsAvailableRequest request, CancellationToken cancellationToken)
    {
        if (!request.Ids.Any()) return null;

        var existingRecords = _writeRepository.RequestedRecords
                .Where(x => request.Ids.Contains(x.Id))
                .ToList();

        var missingRecords = request.Ids.Where(x => !existingRecords.Select(x => x.Id).Contains(x));
        if (missingRecords.Count() > 0)
        {
            throw new Exception($"{missingRecords}");
        }

        existingRecords.ForEach(x => x.UpdateIsAvailable(request.isAvailable));
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return request.Ids;
    }
}
