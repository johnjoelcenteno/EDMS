using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.DeleteRecord;

public record class DeleteRecordRequests(Guid Id) : IRequest<DeleteResponse>;
public class DeleteRecord : IRequestHandler<DeleteRecordRequests, DeleteResponse>
{
    private readonly IWriteRepository _writeRepository;

    public DeleteRecord(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<DeleteResponse> Handle(DeleteRecordRequests request, CancellationToken cancellationToken)
    {
        var record = await _writeRepository.Records.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (record != null)
        {
            _writeRepository.Records.Remove(record);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new DeleteResponse(request.Id);
        }
        return new DeleteResponse(request.Id) { Success = false, Error = $"The request not found {request.Id}" };
    }
}