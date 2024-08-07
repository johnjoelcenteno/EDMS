using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.DeleteRecord;

public record class DeleteRecordRequests(Guid Id) : IRequest<Guid>;
public class DeleteRecord : IRequestHandler<DeleteRecordRequests, Guid>
{
    private readonly IWriteRepository _writeRepository;

    public DeleteRecord(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid> Handle(DeleteRecordRequests request, CancellationToken cancellationToken)
    {
        var record = await _writeRepository.Records.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (record is null) throw new Exception("No records found");

        _writeRepository.Records.Remove(record);
        await _writeRepository.SaveChangesAsync();

        return record.Id;
    }
}