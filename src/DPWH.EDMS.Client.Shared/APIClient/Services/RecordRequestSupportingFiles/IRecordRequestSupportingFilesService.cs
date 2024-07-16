﻿using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles
{
    public interface IRecordRequestSupportingFilesService
    {
        Task<CreateResponse> Upload(FileParameter document, RecordRequestProvidedDocumentTypes? documentType, Guid? documentTypeId);
        Task<CreateResponse> UploadRequestedRecord(FileParameter document, Guid? id);
    }
}