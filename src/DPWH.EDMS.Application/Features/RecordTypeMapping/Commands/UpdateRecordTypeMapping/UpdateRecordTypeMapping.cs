using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;

namespace DPWH.EDMS.Application;


public record class UpdateRecordTypeRequest(Guid Id, UpdateRecordTypeModel model) : IRequest<Guid?>;

public class UpdateRecordTypeMapping : IRequestHandler<UpdateRecordTypeRequest, Guid?>
{
    public UpdateRecordTypeMapping(IWriteRepository writeRepository)
    {
        WriteRepository = writeRepository;
    }

    public IWriteRepository WriteRepository { get; }

    public async Task<Guid?> Handle(UpdateRecordTypeRequest request, CancellationToken cancellationToken)
    {
        var recordType = WriteRepository.RecordTypes.FirstOrDefault(x => x.Id == request.Id);
        if (recordType is null) return null;
        string modifiedBy = "";

        var model = request.model;
        recordType.Update(model.DataLibraryId, model.Division, model.Section, modifiedBy);
        await WriteRepository.SaveChangesAsync(cancellationToken);
        return recordType.Id;
    }
}
