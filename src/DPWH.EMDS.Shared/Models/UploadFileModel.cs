using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.Models;

public class UploadSupportingFileRequestModel
{
    public required FileParameter document { get; set; }
    public required RecordRequestProvidedDocumentTypes? documentType { get; set; }
    public required Guid? documentTypeId { get; set; }
}
