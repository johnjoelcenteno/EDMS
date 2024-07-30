using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.CreateRecord;

public record class CreateRecordCommand(string EmployeeId, Guid RecordTypeId, string RecordName, string RecordUri) : IRequest<CreateResponse>;
internal sealed class CreateRecordCommandHandler(IWriteRepository writeRepository, IReadRepository readRepository, ClaimsPrincipal principal) : IRequestHandler<CreateRecordCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
    {
        var recordType = readRepository.RecordTypesView.FirstOrDefault(r => r.Id == request.RecordTypeId) ?? throw new AppException($"Provided record type is invalid.");

        var record = Record.Create(request.EmployeeId, request.RecordTypeId, request.RecordName, request.RecordUri, principal.GetUserName());
        writeRepository.Records.Add(record);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(record.Id);
    }
}
