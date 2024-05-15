using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Commands.UpdateAsset;

public record UpdateAssetCommand(UpdateAssetRequest UpdateAssetRequest) : IRequest<UpdateAssetResult>;

internal sealed class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, UpdateAssetResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateAssetHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateAssetResult> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var model = request.UpdateAssetRequest;

        var entity = await _repository.Assets
            .Include(a => a.FinancialDetails)
            .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

        if (entity is null)
        {
            throw new AppException($"Property `{model.Id}` not found");
        }

        var propertyStatus = EnumExtensions.GetValueFromDescription<PropertyStatus>(model.PropertyStatus.ToString());

        if (propertyStatus == null)
        {
            throw new ArgumentException($"Invalid Property Status: {model.PropertyStatus}");
        }

        var oldEntity = entity.Clone();

        entity.UpdateDetails(
            model.PropertyId,
            model.Name, model.PropertyStatus,
            model.Region, model.RegionId,
            model.Province, model.ProvinceId,
            model.CityOrMunicipality, model.CityOrMunicipalityId,
            model.Barangay, model.BarangayId,
            model.ZipCode, model.StreetAddress,
            model.LongDegrees, model.LongMinutes, model.LongSeconds, model.LongDirection,
            model.LatDegrees, model.LatMinutes, model.LatSeconds, model.LatDirection,
            model.LotArea, model.FloorArea, model.Floors,
            model.ZonalValue, _principal.GetUserName(),
            model.AttachedAgency, model.Agency, model.Group,
            model.ConstructionType, model.LotStatus, model.BuildingStatus,
            model.BookValue, model.AppraisedValue,
            model.YearConstruction, model.MonthConstruction, model.DayConstruction,
            model.ImplementingOffice, model.RequestingOffice);

        var financial = model.FinancialDetails;

        if (financial != null)
        {
            if (entity.FinancialDetails != null)
            {
                entity.FinancialDetails.Update(
                    financial.PaymentDetails, financial.ORNumber, financial.PaymentDate, financial.AmountPaid,
                    financial.Policy, financial.PolicyNumber, financial.PolicyID, financial.EffectivityStart,
                    financial.Particular, financial.Building, financial.Content,
                    financial.Premium, financial.TotalPremium, financial.Remarks, _principal.GetUserName(),
                    financial.EffectivityEnd);
            }
            else
            {
                entity.FinancialDetails = FinancialDetails.Create(
                    financial.PaymentDetails, financial.ORNumber, financial.PaymentDate,
                    financial.AmountPaid, financial.Policy, financial.PolicyNumber,
                    financial.PolicyID, financial.EffectivityStart, financial.Particular,
                    financial.Building, financial.Content,
                    financial.Premium, financial.TotalPremium, financial.Remarks, _principal.GetUserName(),
                    financial.EffectivityEnd);
            }
        }
        if (entity.Equals(oldEntity)) return new UpdateAssetResult(entity);

        _repository.Assets.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateAssetResult(entity);
    }
}