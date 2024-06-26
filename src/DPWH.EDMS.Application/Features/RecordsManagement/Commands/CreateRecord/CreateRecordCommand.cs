using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.CreateRecord;

public record class CreateRecordCommand(string EmployeeId, Guid RecordTypeId, string RecordName, string RecordUri) : IRequest<CreateResponse>;
internal sealed class CreateRecordCommandHandler(IWriteRepository writeRepository, ClaimsPrincipal principal) : IRequestHandler<CreateRecordCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
    {
        var record = Record.Create(request.EmployeeId, request.RecordTypeId, request.RecordName, request.RecordUri, principal.GetUserName());
        writeRepository.Records.Add(record);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(record.Id);
    }
}
