using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordTypes.Commands;


public record class UpdateRecordTypeRequest(Guid Id, UpdateRecordTypeModel model) : IRequest<Guid?>;

public class UpdateRecordType : IRequestHandler<UpdateRecordTypeRequest, Guid?>
{
    public UpdateRecordType(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        WriteRepository = writeRepository;
        _principal = principal;
    }

    public IWriteRepository WriteRepository { get; }

    private readonly ClaimsPrincipal _principal;

    public async Task<Guid?> Handle(UpdateRecordTypeRequest request, CancellationToken cancellationToken)
    {
        var recordType = WriteRepository.RecordTypes.FirstOrDefault(x => x.Id == request.Id);
        if (recordType is null) return null;
        string modifiedBy = _principal.GetUserName();

        var model = request.model;
        recordType.Update(
            model.Name,
            model.Category,
            model.Section,
            model.Office,
            model.IsActive,
            modifiedBy
        );
        await WriteRepository.SaveChangesAsync(cancellationToken);
        return recordType.Id;
    }
}
