using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using MediatR;

namespace DPWH.EDMS.Application;

public record class CreateRecordTypeRequest(CreateRecordTypeModel Model) : IRequest<Guid>;
public class CreateRecordTypeMapping : IRequestHandler<CreateRecordTypeRequest, Guid>
{
    public IWriteRepository WriteRepository { get; }
    public CreateRecordTypeMapping(IWriteRepository writeRepository)
    {
        WriteRepository = writeRepository;
    }

    public async Task<Guid> Handle(CreateRecordTypeRequest request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        string createdBy = "";
        RecordType recordTypeMapping = RecordType.Create(model.DataLibraryId, model.Division, model.Section, createdBy);

        WriteRepository.RecordTypes.Add(recordTypeMapping);
        await WriteRepository.SaveChangesAsync();
        return recordTypeMapping.Id;
    }
}
