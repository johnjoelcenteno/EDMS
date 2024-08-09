using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateOfficeStatus;

public record UpdateOfficeStatus(Guid Id, string Status);