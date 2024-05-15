using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateBuildingComponent;

public record UpdateBuildingComponent
{
    public string SubCategory { get; set; }
    public bool ForRepair { get; set; }
    public int? Rating { get; set; }
    public string? Particular { get; set; }
}
public class UpdateBuildingComponentCommand : IRequest<UpdateBuildingComponentResult>
{
    public Guid InspectionRequestId { get; set; }
    public string Category { get; set; }
    public string Status { get; set; }
    public List<UpdateBuildingComponent>? SubCategories { get; set; }
}

internal class UpdateBuildingComponentHandler : IRequestHandler<UpdateBuildingComponentCommand, UpdateBuildingComponentResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateBuildingComponentHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateBuildingComponentResult> Handle(UpdateBuildingComponentCommand requests, CancellationToken cancellationToken)
    {
        var entity = await _repository.InspectionRequests
            .Include(i => i.InspectionRequestBuildingComponents)
            .FirstOrDefaultAsync(x => x.Id == requests.InspectionRequestId, cancellationToken)
            ?? throw new AppException($"Inspection request `{requests.InspectionRequestId}` not found");

        var inspectionRequestBuildingComponents = await _repository.InspectionRequestBuildingComponents
             .Where(x => x.InspectionRequestId == entity.Id)
             .ToListAsync(cancellationToken);

        var existingMainComponent = inspectionRequestBuildingComponents.FirstOrDefault(x => x.Category == requests.Category);

        foreach (var subCategory in requests.SubCategories)
        {
            if (existingMainComponent != null)
            {
                if (existingMainComponent.Category != null && existingMainComponent.SubCategory == null)
                {
                    // Updates first component in db then create for the succeeding components.
                    existingMainComponent.UpdateDetails(subCategory.SubCategory, subCategory.ForRepair, subCategory.Rating, subCategory.Particular, _principal.GetUserName());
                    _repository.InspectionRequestBuildingComponents.Update(existingMainComponent);
                }
                else
                {
                    var existingSubComponent = inspectionRequestBuildingComponents.FirstOrDefault(x => x.SubCategory == subCategory.SubCategory && x.Category == requests.Category);
                    if (existingSubComponent == null)
                    { // Creates subcomponents
                        existingSubComponent = InspectionRequestBuildingComponent.CreateComponents(entity, requests.Category, subCategory.SubCategory, subCategory.ForRepair, subCategory.Rating, subCategory.Particular, _principal.GetUserName());
                        _repository.InspectionRequestBuildingComponents.Add(existingSubComponent);
                    }
                    else
                    { // Updating details of existing components
                        foreach (var buildingComponent in inspectionRequestBuildingComponents)
                        {
                            if (subCategory.SubCategory == buildingComponent.SubCategory && requests.Category == buildingComponent.Category)
                            {
                                var existingSubCategory = inspectionRequestBuildingComponents.FirstOrDefault(x => x.SubCategory == subCategory.SubCategory && x.Category == buildingComponent.Category);

                                if (existingSubCategory != null)
                                {
                                    existingSubCategory.UpdateDetails(subCategory.SubCategory, subCategory.ForRepair, subCategory.Rating, subCategory.Particular, _principal.GetUserName());
                                    _repository.InspectionRequestBuildingComponents.Update(existingSubCategory);
                                }
                            }
                        }
                    }
                }
            }
            else
            { // Creates new custom components
                var customComponents = InspectionRequestBuildingComponent.CreateComponents(entity, requests.Category, subCategory.SubCategory, subCategory.ForRepair, subCategory.Rating, subCategory.Particular, _principal.GetUserName());
                _repository.InspectionRequestBuildingComponents.Add(customComponents);
            }
        }
        var status = (InspectionRequestStatus)Enum.Parse(typeof(InspectionRequestStatus), requests.Status);

        entity.UpdateStatus(status, _principal.GetUserName()); // Update IR - PLI status 
        _repository.InspectionRequests.Update(entity);

        await _repository.SaveChangesAsync(cancellationToken);
        return new UpdateBuildingComponentResult(entity);
    }
}
