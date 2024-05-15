using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRates;

public record CreateRentalRatesCommand : IRequest<Guid>
{
    public bool IsDraft { get; set; }
    public Guid RentalRatePropertyId { get; set; }
    public string Status { get; set; }
    public int? Accessibility { get; set; }
    public int? Topography { get; set; }
    public int? Sidewalk { get; set; }
    public int? Parking { get; set; }
    public int? EconomicPotentiality { get; set; }
    public int? LandClassification { get; set; }
    public int? OtherAmenities { get; set; }
    public int? PrevailingRentalRates { get; set; }
    public int? Sanitation { get; set; }
    public int? AdverseInfluence { get; set; }
    public int? PropertyUtilization { get; set; }
    public int? PoliceFireStation { get; set; }
    public int? Cafeteria { get; set; }
    public int? Banking { get; set; }
    public int? StructuralCondition { get; set; }
    public int? Module { get; set; }
    public int? RoomArrangement { get; set; }
    public int? Circulation { get; set; }
    public int? LightAndVentilation { get; set; }
    public int? SpaceRequirements { get; set; }
    public int? WaterSupply { get; set; }
    public int? LightingSystem { get; set; }
    public int? Elevators { get; set; }
    public int? Maintenance { get; set; }
    public int? FireEscapes { get; set; }
    public int? FireFightingEquipments { get; set; }
    public int? Alternatives { get; set; }
    public int? Janitorial { get; set; }
    public int? Airconditoning { get; set; }
    public int? RepairMaintenance { get; set; }
    public int? WaterLightConsumption { get; set; }
    public int? SecuredParkingSpace { get; set; }
    public decimal? FactorValue { get; set; }
    public decimal? DepreciationValue { get; set; }
    public decimal? CapitalizationRate { get; set; }
    public decimal? UnitConstructionCost { get; set; }
    public decimal? ReproductionCost { get; set; }
    public decimal? FormulaRate { get; set; }
    public decimal? RentalRates { get; set; }
    public decimal? Area { get; set; }
    public decimal? MonthlyRental { get; set; }
    public decimal? DepreciationRatePercentage { get; set; }
    public decimal? CapitalizationRatePercentage { get; set; }
}
internal sealed class CreateRentalRatesHandler : IRequestHandler<CreateRentalRatesCommand, Guid>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateRentalRatesHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<Guid> Handle(CreateRentalRatesCommand request, CancellationToken cancellationToken)
    {
        var rentalRateProperty = await _repository.RentalRateProperty.FirstOrDefaultAsync(x => x.Id == request.RentalRatePropertyId, cancellationToken)
                    ?? throw new AppException($"Rental Rate Property `{request.RentalRatePropertyId}` not found");

        var rentalRate = await _repository.RentalRates.FirstOrDefaultAsync(x => x.RentalRatePropertyId == request.RentalRatePropertyId, cancellationToken);

        if (rentalRate != null)
        {
            throw new AppException($"Rental Rate `{request.RentalRatePropertyId}` already exists");
        }

        request.Status = !request.IsDraft ? InspectionRequestStatus.Submitted.ToString() : InspectionRequestStatus.Assigned.ToString();

        if (!request.IsDraft)
        {
            var inspection = _repository.InspectionRequests.FirstOrDefault(x => x.RentalRatePropertyId == request.RentalRatePropertyId);
            inspection?.UpdateStatus(InspectionRequestStatus.Submitted, _principal.GetUserName());
            _repository.InspectionRequests.Update(inspection);
        }


        var entity = RentalRate.Create(
                rentalRateProperty,
                request.Status,
                request.Accessibility,
                request.Topography,
                request.Sidewalk,
                request.Parking,
                request.EconomicPotentiality,
                request.LandClassification,
                request.OtherAmenities,
                request.PrevailingRentalRates,
                request.Sanitation,
                request.AdverseInfluence,
                request.PropertyUtilization,
                request.PoliceFireStation,
                request.Cafeteria,
                request.Banking,
                request.StructuralCondition,
                request.Module,
                request.RoomArrangement,
                request.Circulation,
                request.LightAndVentilation,
                request.SpaceRequirements,
                request.WaterSupply,
                request.LightingSystem,
                request.Elevators,
                request.FireEscapes,
                request.FireFightingEquipments,
                request.Maintenance,
                request.Alternatives,
                request.Janitorial,
                request.Airconditoning,
                request.RepairMaintenance,
                request.WaterLightConsumption,
                request.SecuredParkingSpace,
                request.FactorValue,
                request.DepreciationValue,
                request.CapitalizationRate,
                request.UnitConstructionCost,
                request.ReproductionCost,
                request.FormulaRate,
                request.RentalRates,
                request.Area,
                request.MonthlyRental,
                request.DepreciationRatePercentage,
                request.CapitalizationRatePercentage,
                _principal.GetUserName());


        await _repository.RentalRates.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
