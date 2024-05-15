using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringBuildingComponentImage;

internal sealed class CreateProjectMonitoringBuildingComponentImage : IRequestHandler<SaveProjectMonitoringBuildingComponentImage, SaveProjectMonitoringBuildingComponentImageResponse>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateProjectMonitoringBuildingComponentImage(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _repository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveProjectMonitoringBuildingComponentImageResponse> Handle(SaveProjectMonitoringBuildingComponentImage request, CancellationToken cancellationToken)
    {
        var model = request.Details;
        var fileSize = model.File.Length;

        var projectMonitoringImage = _repository.InspectionRequestProjectMonitoringScopesImage.FirstOrDefault(x => x.Id == model.Id);

        if (projectMonitoringImage is null)
        {
            projectMonitoringImage = InspectionRequestProjectMonitoringScopesImage.Create(model.Id ?? Guid.NewGuid(), model.ProjectMonitoringBuildingComponentId, model.Filename, fileSize, model.Uri, _principal.GetUserName());
            _repository.InspectionRequestProjectMonitoringScopesImage.Add(projectMonitoringImage);
        }
        else
        {
            projectMonitoringImage.Update(model.Filename, fileSize, model.Uri, _principal.GetUserName());
        }
        await _repository.SaveChangesAsync(cancellationToken);

        return new SaveProjectMonitoringBuildingComponentImageResponse(projectMonitoringImage.Id);
    }
}
