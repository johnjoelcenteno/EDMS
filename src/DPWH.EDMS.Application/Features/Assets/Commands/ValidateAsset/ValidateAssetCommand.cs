using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Assets.Commands.ValidateAsset;

public record ValidateAssetCommand(ValidateAssetRequest ValidateAssetRequest) : IRequest<ValidateAssetResult>;
internal sealed class ValidateAssetHandler : IRequestHandler<ValidateAssetCommand, ValidateAssetResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public ValidateAssetHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }
    public async Task<ValidateAssetResult> Handle(ValidateAssetCommand request, CancellationToken cancellationToken)
    {
        var model = request.ValidateAssetRequest;

        var entity = await _repository.Assets
            .Include(a => a.FinancialDetails)
            .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken) ?? throw new AppException($"Property `{model.Id}` not found");

        var propertyStatus = EnumExtensions.GetValueFromDescription<PropertyStatus>(model.PropertyStatus.ToString());

        if (propertyStatus == null) throw new ArgumentException($"Invalid Property Status: {model.PropertyStatus}");

        var oldEntity = entity.Clone();
        entity.Update(
            model.PropertyId,
            model.Name, model.PropertyStatus,
            model.Region, model.RegionId,
            model.Province, model.ProvinceId,
            model.CityOrMunicipality, model.CityOrMunicipalityId,
            model.Barangay, model.BarangayId,
            model.ZipCode, model.StreetAddress,
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
        if (entity.Equals(oldEntity)) return new ValidateAssetResult(entity);

        var status = EnumExtensions.GetValueFromDescription<AssetStatus>("For Approval");
        entity.Update(status, _principal.GetUserName());

        _repository.Assets.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ValidateAssetResult(entity);
    }
}
