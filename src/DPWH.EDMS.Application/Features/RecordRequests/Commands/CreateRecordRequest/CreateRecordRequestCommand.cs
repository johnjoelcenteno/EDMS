using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Lookups.Queries;
using DPWH.EDMS.Application.Features.RecordTypes;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;

public record class CreateRecordRequestCommand(CreateRecordRequest Model) : IRequest<CreateResponse>;
internal sealed class CreateRecordRequestCommandHandler(IWriteRepository writeRepository, IReadRepository readRepository,
    ClaimsPrincipal principal, IControlNumberGeneratorService _generatorService) : IRequestHandler<CreateRecordRequestCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRecordRequestCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var isAuthorizedRep = false;

        var claimantType = EnumExtensions.GetValueFromDescription<ClaimantTypes>(model.Claimant);

        AuthorizedRepresentative? representative = null;
        if (claimantType == ClaimantTypes.AuthorizedRepresentative)
        {
            representative = AuthorizedRepresentative.Create(model.AuthorizedRepresentative, model.SupportingFileValidId, model.SupportingFileAuthorizationDocumentId);

            isAuthorizedRep = true;
        }

        //Optional for now: Make sure requested record type is valid
        var recordTypes = readRepository.RecordTypesView
            .Where(d => d.Category == RecordTypesCategory.Issuances.GetDescription() ||
                        d.Category == RecordTypesCategory.EmployeeRecords.GetDescription())
            //.Select(d => new GetLookupResult(d.Id, d.Name))
            .ToList();

        var inputRecordTypes = model.RequestedRecords;

        bool allExist = inputRecordTypes.All(x => recordTypes.Select(r => r.Id).Contains(x));

        if (!allExist)
        {
            throw new AppException($"Provided RequestedRecords contains invalid data.");
        }

        var requestNumber = await _generatorService.Generate(DateTimeOffset.Now, cancellationToken);

        var recordRequest = RecordRequest.Create(requestNumber, model.EmployeeNumber, claimantType,
            model.DateRequested, representative, model.Purpose, principal.GetUserName(), model.FullName);


        foreach (var providedRequestedRecord in model.RequestedRecords)
        {
            var recordType = recordTypes.FirstOrDefault(x => x.Id == providedRequestedRecord);

            var requestedRecord = RequestedRecord.Create(recordRequest.Id, providedRequestedRecord, recordType.Name, recordType.Office);
            recordRequest.RequestedRecords.Add(requestedRecord);
        }

        if (isAuthorizedRep)
        {
            //If Claimant is Representative, we need atleast valid ID            
            if (model.SupportingFileValidId is not null && model.SupportingFileValidId != default)
            {
                //get ValidId and update reference
                var validId = writeRepository.RecordRequestDocuments.FirstOrDefault(d => d.Id == model.SupportingFileValidId);

                if (validId is not null)
                {
                    validId.RecordRequestId = recordRequest.Id;
                }
            }

            if (model.SupportingFileAuthorizationDocumentId is not null && model.SupportingFileAuthorizationDocumentId != default)
            {
                //get AuthorizationDocuments and update reference
                var supportingDocument = writeRepository.RecordRequestDocuments.FirstOrDefault(d => d.Id == model.SupportingFileAuthorizationDocumentId);

                if (supportingDocument is not null)
                {
                    supportingDocument.RecordRequestId = recordRequest.Id;
                }
            }
        }

        writeRepository.RecordRequests.Add(recordRequest);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(recordRequest.Id);
    }
}
