using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Application.Features.RecordRequests.Mappers;

public static class RecordRequestMappers
{
    public static RecordRequestModel MapToModel(RecordRequest entity)
    {
        return new RecordRequestModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            ControlNumber = entity.ControlNumber,
            EmployeeNumber = entity.EmployeeNumber,
            Email = entity.Email,
            ClaimantType = entity.ClaimantType,
            DateRequested = entity.DateRequested,
            AuthorizedRepresentative = entity.AuthorizedRepresentative.ValidId is not null ? new AuthorizedRepresentativeModel
            {
                SupportingFileValidId = entity.AuthorizedRepresentative.ValidId,
                SupportingFileAuthorizationDocumentId = entity.AuthorizedRepresentative.AuthorizationDocumentId,
                RepresentativeName = entity.AuthorizedRepresentative.RepresentativeName,
                ValidId = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).DocumentTypeId,
                ValidIdName = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).Name,
                ValidIdUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).Uri,
                AuthorizationDocumentId = (entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString()).DocumentTypeId,
                AuthorizationDocumentName = (entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString()).Name,
                AuthorizationDocumentUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString()).Uri
            } : new(),
            RequestedRecords = entity.RequestedRecords.Select(rr => new RequestedRecordModel(rr.Id, rr.RecordTypeId, rr.RecordType, rr.Office, rr.Status, rr.IsAvailable, rr.Uri)).ToList(),
            Purpose = entity.Purpose,
            Remarks = entity.Remarks,
            Status = entity.Status,
            Files = entity.Files?.Select(entityFile => new RecordRequestDocumentModel
            {
                Id = entityFile.Id,
                Name = entityFile.Name,
                Filename = entityFile.Filename,
                Type = entityFile.Type,
                DocumentTypeId = entityFile.DocumentTypeId,
                FileSize = entityFile.FileSize,
                Uri = entityFile.Uri
            }).ToList() ?? new List<RecordRequestDocumentModel>()
        };
    }
}
