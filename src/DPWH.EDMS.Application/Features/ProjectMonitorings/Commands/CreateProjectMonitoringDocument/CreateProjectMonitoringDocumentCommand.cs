using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringDocument;

public record CreateProjectMonitoringCommand(CreateProjectMonitoringDocumentRequest Request) : IRequest<Guid>;
internal sealed class CreateProjectMonitoringDocumentHandler : IRequestHandler<CreateProjectMonitoringCommand, Guid>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public CreateProjectMonitoringDocumentHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<Guid> Handle(CreateProjectMonitoringCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        var projectMonitoring = _writeRepository.ProjectMonitoring.FirstOrDefault(i => i.Id == model.ProjectMonitoringId);
        var projectMonitoringFile = _writeRepository.ProjectMonitoringDocuments.FirstOrDefault(i => i.Id == model.Id);

        if (projectMonitoringFile is null)
        {
            projectMonitoringFile = ProjectMonitoringDocument.Create(model.Id ?? Guid.NewGuid(), projectMonitoring.Id, model.Name, model.Group, model.Filename, model.Category, fileSize, model.Uri, _principal.GetUserName());
            _writeRepository.ProjectMonitoringDocuments.Add(projectMonitoringFile);
        }
        else
        {
            projectMonitoringFile.Update(model.Name, model.Filename, model.Category, model.Group, fileSize, model.Uri, _principal.GetUserName());
        }
        await _writeRepository.SaveChangesAsync(cancellationToken);

        return projectMonitoringFile.Id;
    }
}
