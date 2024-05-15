using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequestDocument;

public class SaveMaintenanceRequestDocumentResponse : IdResponse
{
    public SaveMaintenanceRequestDocumentResponse(Guid id) : base(id) { }

}

public class SaveMaintenanceRequestDocument : IRequest<SaveMaintenanceRequestDocumentResponse>
{
    public SaveMaintenanceRequestDocument(CreateMaintenanceRequestDocumentRequest request)
    {
        Details = request;
    }

    [Required]
    public CreateMaintenanceRequestDocumentRequest Details { get; }
}


