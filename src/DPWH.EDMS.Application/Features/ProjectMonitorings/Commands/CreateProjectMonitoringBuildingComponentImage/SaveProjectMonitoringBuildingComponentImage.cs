using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestBuildingComponentImage;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringBuildingComponentImage;

public class SaveProjectMonitoringBuildingComponentImageResponse : IdResponse
{
    public SaveProjectMonitoringBuildingComponentImageResponse(Guid id) : base(id) { }
}

public class SaveProjectMonitoringBuildingComponentImage : IRequest<SaveProjectMonitoringBuildingComponentImageResponse>
{
    public SaveProjectMonitoringBuildingComponentImage(CreateProjectMonitoringBuildingComponentImageRequest request)
    {
        Details = request;
    }

    [Required]
    public CreateProjectMonitoringBuildingComponentImageRequest Details { get; }
}

