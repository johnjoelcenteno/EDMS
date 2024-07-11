using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordTypes.Commands.DeleteRecordType;

public record class DeleteRecordTypeCommand(Guid Id) : IRequest<Guid>;
internal sealed class DeleteRecordTypeCommandHandler(IWriteRepository writeRepository): IRequestHandler<DeleteRecordTypeCommand, Guid>
{    
    public async Task<Guid> Handle(DeleteRecordTypeCommand request, CancellationToken cancellationToken)
    {
        var recordType = await writeRepository.RecordTypes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (recordType is null) return request.Id;

        writeRepository.RecordTypes.Remove(recordType);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
