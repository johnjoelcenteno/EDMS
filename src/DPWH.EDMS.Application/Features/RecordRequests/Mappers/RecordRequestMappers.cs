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
            ControlNumber = entity.ControlNumber,
            EmployeeNumber = entity.EmployeeNumber,
            IsActiveEmployee = entity.IsActiveEmployee,
            ClaimantType = entity.ClaimantType,
            DateRequested = entity.DateRequested,
            AuthorizedRepresentative = entity.AuthorizedRepresentative != null ? new AuthorizedRepresentativeModel
            {                
                RepresentativeName = entity.AuthorizedRepresentative.RepresentativeName,
                ValidId = entity.AuthorizedRepresentative.ValidId,
                ValidIdUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).Uri,
                SupportingDocument = entity.AuthorizedRepresentative.SupportingDocument,
                SupportingDocumentUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.SupportingDocument.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.SupportingDocument.ToString()).Uri
            } : null,
            //RequestedRecord = entity.RequestedRecord,
            Purpose = entity.Purpose,
            Status = entity.Status,
            Files = entity.Files?.Select(entityFile => new RecordRequestDocumentModel
            {
                Id = entityFile.Id,                
                Name = entityFile.Name,
                Filename = entityFile.Filename,
                Type = entityFile.Type,
                FileSize = entityFile.FileSize,
                Uri = entityFile.Uri
            }).ToList() ?? new List<RecordRequestDocumentModel>()
        };
    }
}
