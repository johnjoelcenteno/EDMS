using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordsRequestDocumentStatus;

public record UpdateRecordsRequestDocumentStatus(Guid Id, string Status);