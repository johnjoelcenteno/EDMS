using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application;

public record class DeleteRecordTypeRequest(Guid Id) : IRequest<Guid?>;
public class DeleteRecordTypeMapping : IRequestHandler<DeleteRecordTypeRequest, Guid?>
{
    public IWriteRepository WriteRepository { get; }
    public DeleteRecordTypeMapping(IWriteRepository writeRepository)
    {
        WriteRepository = writeRepository;
    }

    public async Task<Guid?> Handle(DeleteRecordTypeRequest request, CancellationToken cancellationToken)
    {
        var recordType = await WriteRepository.RecordTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (recordType is null) return null;

        WriteRepository.RecordTypes.Remove(recordType);
        await WriteRepository.SaveChangesAsync(cancellationToken);
        return recordType.Id;
    }
}
