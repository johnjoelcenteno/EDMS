using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequest;

public record CreateMaintenanceRequestCommand : IRequest<Guid>
{
    public required Guid AssetId { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public required IEnumerable<string> BuildingComponents { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }
    public decimal RequestedAmount { get; set; }
    public string PurposeProjectName { get; set; }
}
internal sealed class CreateMaintenanceRequestHandler : IRequestHandler<CreateMaintenanceRequestCommand, Guid>
{
    private readonly ILogger _logger;
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly IRequestNumberGeneratorService _generatorService;


    public CreateMaintenanceRequestHandler(ILogger<CreateMaintenanceRequestHandler> logger, IWriteRepository repository, ClaimsPrincipal principal, IRequestNumberGeneratorService generatorService)
    {
        _logger = logger;
        _repository = repository;
        _principal = principal;
        _generatorService = generatorService;
    }

    public async Task<Guid> Handle(CreateMaintenanceRequestCommand request, CancellationToken cancellationToken)
    {
        var asset = await _repository.Assets.SingleOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);

        if (asset is null)
        {
            _logger.LogError("Asset `{AssetId}` not found", request.AssetId);
            throw new AppException("Asset not found");
        }

        var buildingComponents = await _repository
             .BuildingComponents
             .AsNoTracking()
             .ToListAsync(cancellationToken);

        var requestNumber = await _generatorService.Generate(DateTimeOffset.Now, cancellationToken);
        var status = (MaintenanceRequestStatus)Enum.Parse(typeof(MaintenanceRequestStatus), request.Status);

        var entity = MaintenanceRequest.Create(
           asset.Id,
           status,
           request.Purpose,
           request.PhotosPerArea,
           request.IsPhotosRequired,
           request.FurtherInstructions,
           request.RequestedAmount,
           request.PurposeProjectName,
           requestNumber,
           _principal.GetUserName());

        entity.MaintenanceRequestBuildingComponents = request
          .BuildingComponents
          .Select(c => MaintenanceRequestBuildingComponent.Create(
              entity,
              buildingComponents.First(bc => bc.Name == c).Name,
              null,
              false,
              0,
              null,
              _principal.GetUserName()))
          .ToList();

        await _repository.MaintenanceRequests.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
