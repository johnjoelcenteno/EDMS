using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestBuildingComponentImage;

public class SaveInspectionRequestBuildingComponentImageResponse : IdResponse
{
    public SaveInspectionRequestBuildingComponentImageResponse(Guid id) : base(id) { }
}

public class SaveInspectionRequestBuildingComponentImage : IRequest<SaveInspectionRequestBuildingComponentImageResponse>
{
    public SaveInspectionRequestBuildingComponentImage(SaveInspectionRequestBuildingComponentImageRequest request)
    {
        Details = request;
    }

    [Required]
    public SaveInspectionRequestBuildingComponentImageRequest Details { get; }
}
