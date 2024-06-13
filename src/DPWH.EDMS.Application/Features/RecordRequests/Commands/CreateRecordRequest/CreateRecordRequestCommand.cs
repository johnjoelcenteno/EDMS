using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetRecordTypes;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Linq;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;

public record class CreateRecordRequestCommand(CreateRecordRequest Model) : IRequest<CreateResponse>;
internal sealed class CreateRecordRequestCommandHandler(IWriteRepository writeRepository, IReadRepository readRepository, ClaimsPrincipal principal) : IRequestHandler<CreateRecordRequestCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRecordRequestCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var claimantType = EnumExtensions.GetValueFromDescription<ClaimantTypes>(model.Claimant);

        AuthorizedRepresentative? representative = null;
        if(claimantType == ClaimantTypes.AuthorizedRepresentative)
        {
            representative = AuthorizedRepresentative.Create(model.AuthorizedRepresentative, 
                model.ValidId, model.SupportingDocument, model.SupportingDocument, model.SupportingDocument);
        }

        //Optional for now: Make sure requested record type is valid
        var recordTypes = readRepository.DataLibrariesView
            .Where(d => d.Type == WellKnownDataLibraryTypes.RecordTypes)
            .Select(d => new GetRecordTypesResult(d.Id, d.Value))
            .ToList();

        var inputRecordTypes = model.RequestedRecords;

        bool allExist = inputRecordTypes.All(x => recordTypes.Select(r => r.Id).Contains(x));

        if (!allExist)
        {
            throw new AppException($"Provided RequestedRecords contains invalid data.");
        }

        var recordRequest = RecordRequest.Create(model.EmployeeNumber, model.ControlNumber, claimantType,
            model.IsActiveEmployee, model.DateRequested, representative, model.Purpose, principal.GetUserName());

        
        foreach (var providedRequestedRecord in model.RequestedRecords)
        {
            var requestedRecord = RequestedRecord.Create(recordRequest.Id, providedRequestedRecord, recordTypes.FirstOrDefault(x => x.Id == providedRequestedRecord).Name);
            recordRequest.RequestedRecords.Add(requestedRecord);
        }

        writeRepository.RecordRequests.Add(recordRequest);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(recordRequest.Id);
    }
}
