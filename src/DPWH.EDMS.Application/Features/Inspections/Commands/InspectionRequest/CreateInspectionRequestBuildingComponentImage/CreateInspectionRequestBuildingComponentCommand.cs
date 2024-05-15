using DPWH.EDMS.Domain.Entities;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestBuildingComponentImage;

public class CreateInspectionRequestBuildingComponentCommand : IRequestHandler<SaveInspectionRequestBuildingComponentImage, SaveInspectionRequestBuildingComponentImageResponse>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public CreateInspectionRequestBuildingComponentCommand(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveInspectionRequestBuildingComponentImageResponse> Handle(SaveInspectionRequestBuildingComponentImage request, CancellationToken cancellationToken)
    {
        var model = request.Details;
        long fileSize = model.File.Length;

        var assetImage = _writeRepository.InspectionRequestBuildingComponentImage.FirstOrDefault(i => i.Id == model.Id);
        if (assetImage is null)
        {
            assetImage = InspectionRequestBuildingComponentImage.Create(model.Id ?? Guid.NewGuid(), model.InspectionRequestBuildingComponentId, model.Filename, fileSize, model.Uri, _principal.GetUserName());
            _writeRepository.InspectionRequestBuildingComponentImage.Add(assetImage);
        }
        else
        {
            assetImage.Update(model.Filename, fileSize, model.Uri, _principal.GetUserName());
        }

        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveInspectionRequestBuildingComponentImageResponse(assetImage.Id);
    }
}
