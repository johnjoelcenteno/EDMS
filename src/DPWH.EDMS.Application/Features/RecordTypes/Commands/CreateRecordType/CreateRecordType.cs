using System.Diagnostics.Metrics;
using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
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

        if (model.Code is null && model.Category == RecordTypesCategory.PersonalRecords.GetDescription() || model.Code is not null && model.Category != RecordTypesCategory.PersonalRecords.GetDescription())
        {
            throw new AppException($"Only Personal Records are required to have a Document Code.");
        }
        string createdBy = _principal.GetUserName();
        RecordType recordTypeMapping = RecordType.Create(
            model.Name,
            model.Category,
            model.Section,
            model.Office,
            model.IsActive,
            model.Code,
            createdBy
        );

        WriteRepository.RecordTypes.Add(recordTypeMapping);
        await WriteRepository.SaveChangesAsync();
        return recordTypeMapping.Id;
    }
}
