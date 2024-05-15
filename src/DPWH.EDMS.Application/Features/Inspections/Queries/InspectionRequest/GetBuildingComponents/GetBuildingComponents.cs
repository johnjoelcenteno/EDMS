using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponents;

public record GetBuildingComponents : IRequest<IEnumerable<GetBuildingComponentsResult>>;

internal sealed class GetBuildingComponentsHandler : IRequestHandler<GetBuildingComponents, IEnumerable<GetBuildingComponentsResult>>
{
    private readonly IReadRepository _repository;

    public GetBuildingComponentsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetBuildingComponentsResult>> Handle(GetBuildingComponents request, CancellationToken cancellationToken)
    {
        var buildingComponents = await _repository.BuildingComponentsView.ToListAsync(cancellationToken);

        return buildingComponents
            .Where(h => h.Type == "Header")
            .Select(h => new GetBuildingComponentsResult
            {
                Ordinal = h.Ordinal,
                Id = h.CategoryId,
                ItemNo = h.ItemNo,
                Name = h.Name,
                Categories = buildingComponents
                    .Where(c => c.Type == "Category" && c.ParentId == h.CategoryId)
                    .Select(c => new GetBuildingComponentsResultCategory
                    {
                        Ordinal = c.Ordinal,
                        Id = c.CategoryId,
                        ItemNo = c.ItemNo,
                        Name = c.Name,
                        Subcategories = buildingComponents
                            .Where(s => s.Type == "Subcategory" && s.ParentId == c.CategoryId)
                            .Select(s => new GetBuildingComponentsResultSubcategory
                            {
                                Ordinal = s.Ordinal,
                                Id = s.CategoryId,
                                Name = s.Name,
                                Items = buildingComponents
                                    .Where(i => i.Type == "Item" && s.CategoryId.Contains(i.ParentId))
                                    .Select(i => new GetBuildingComponentsResultItems
                                    {
                                        ItemId = $"{i.ItemNo} {(i.Suffix != null ? "- " + i.Suffix : "")}",
                                        ItemNo = i.ItemNo,
                                        Suffix = i.Suffix,
                                        Description = i.Description,
                                        Thickness = i.Thickness,
                                        Class = i.Class,
                                        Others = i.Others,
                                        UnitOfMeasure = i.UnitOfMeasure
                                    })
                            })
                            .OrderBy(s => s.Ordinal)
                    })
                    .OrderBy(c => c.Ordinal)
            })
            .OrderBy(h => h.Ordinal);
    }
}
