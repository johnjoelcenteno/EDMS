using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordTypes.Commands;

public record class CreateRecordTypeRequest(CreateRecordTypeModel Model) : IRequest<Guid>;
public class CreateRecordType : IRequestHandler<CreateRecordTypeRequest, Guid>
{
    public IWriteRepository WriteRepository { get; }

    private readonly ClaimsPrincipal _principal;

    public CreateRecordType(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        WriteRepository = writeRepository;
        _principal = principal;
    }

    public async Task<Guid> Handle(CreateRecordTypeRequest request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        string createdBy = _principal.GetUserName();
        RecordType recordTypeMapping = RecordType.Create(
            model.Name,
            model.Category,
            model.Section,
            model.Office,
            model.IsActive,
            createdBy
        );

        WriteRepository.RecordTypes.Add(recordTypeMapping);
        await WriteRepository.SaveChangesAsync();
        return recordTypeMapping.Id;
    }
}
