using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequestDocument;

public record CreateMaintenanceRequestDocumentCommand(CreateMaintenanceRequestDocumentRequest Request) : IRequest<SaveMaintenanceRequestDocumentResponse>;
public class CreateMaintenanceRequestDocumentHandler : IRequestHandler<CreateMaintenanceRequestDocumentCommand, SaveMaintenanceRequestDocumentResponse>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public CreateMaintenanceRequestDocumentHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveMaintenanceRequestDocumentResponse> Handle(CreateMaintenanceRequestDocumentCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        var maintenanceRequest = _writeRepository.MaintenanceRequests.FirstOrDefault(i => i.Id == model.MaintenanceRequestId);
        var maintenanceRequestFile = _writeRepository.MaintenanceRequestDocuments.FirstOrDefault(i => i.Id == model.Id);

        var group = EnumExtensions.GetValueFromDescription<MaintenanceDocumentType>(model.Group);

        if (group == null)
        {
            throw new AppException($"Invalid Group : {model.Group}");
        }


        if (maintenanceRequestFile is null)
        {
            maintenanceRequestFile = MaintenanceRequestDocument.Create(model.Id ?? Guid.NewGuid(), maintenanceRequest.Id, model.Name, group, model.Filename, model.Category, fileSize, model.Uri, _principal.GetUserName());
            _writeRepository.MaintenanceRequestDocuments.Add(maintenanceRequestFile);
        }
        else
        {
            maintenanceRequestFile.Update(model.Name, model.Filename, model.Category, group.ToString(), fileSize, model.Uri, _principal.GetUserName());
        }
        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveMaintenanceRequestDocumentResponse(maintenanceRequestFile.Id);
    }
}
