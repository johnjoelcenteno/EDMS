﻿using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Extensions;
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


                ValidIdName = EnumExtensions.GetDescriptionFromValue<RecordRequestProvidedDocumentTypes>(entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).Type),



                ValidIdUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.ValidId.ToString()).Uri,
                SupportingDocument = entity.AuthorizedRepresentative.SupportingDocument,


                SupportingDocumentName = EnumExtensions.GetDescriptionFromValue<RecordRequestProvidedDocumentTypes>(entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString()).Type),


                SupportingDocumentUri = (entity.Files?.FirstOrDefault(f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString())) == null ? null : entity.Files?.FirstOrDefault(predicate: f => f.Type == RecordRequestProvidedDocumentTypes.AuthorizationDocument.ToString()).Uri
            } : null,
            RequestedRecords = entity.RequestedRecords.Select(rr => new RequestedRecordModel(rr.RecordTypeId, rr.RecordType)).ToList(),
            Purpose = entity.Purpose,
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
