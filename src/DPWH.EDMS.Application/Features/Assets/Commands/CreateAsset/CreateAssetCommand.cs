using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.Assets.Commands.CreateAsset;

public record CreateAssetCommand(CreateAssetRequest CreateAssetRequest) : IRequest<CreateResponse>;

internal sealed class CreateAssetHandler : IRequestHandler<CreateAssetCommand, CreateResponse>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly IBuildingIdSequenceGeneratorService _generatorService;

    public CreateAssetHandler(
        IWriteRepository repository,
        ClaimsPrincipal principal,
        IBuildingIdSequenceGeneratorService generatorService)
    {
        _repository = repository;
        _principal = principal;
        _generatorService = generatorService;
    }

    public async Task<CreateResponse> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var model = request.CreateAssetRequest;
        var propertyStatus = EnumExtensions.GetValueFromDescription<PropertyStatus>(model.PropertyStatus);

        if (propertyStatus == null)
        {
            throw new ArgumentException($"Invalid Property Status: {model.PropertyStatus}");
        }

        var buildingId = await _generatorService.Generate(model.Agency, model.RequestingOffice, cancellationToken);

        var entity = Asset.Create(
            model.PropertyId, model.Name, model.PropertyStatus,
            model.RegionId, model.Region,
            model.ProvinceId, model.Province,
            model.CityOrMunicipalityId, model.CityOrMunicipality,
            model.BarangayId, model.Barangay,
            model.ZipCode, model.StreetAddress,
            model.LongDegrees, model.LongMinutes, model.LongSeconds, model.LongDirection,
            model.LatDegrees, model.LatMinutes, model.LatSeconds, model.LatDirection,
            model.LotArea, model.FloorArea, model.Floors, model.ZonalValue,
            _principal.GetUserName(), buildingId, model.AttachedAgency, model.Agency, model.Group,
            model.ConstructionType, model.LotStatus, model.BuildingStatus, model.BookValue, model.AppraisedValue,
            model.YearConstruction, model.MonthConstruction, model.DayConstruction,
            model.OldId, model.Remarks, model.ImplementingOffice, model.RequestingOffice);

        var financial = model.FinancialDetails;

        if (financial is not null)
        {
            entity.FinancialDetails = FinancialDetails.Create(
                financial.PaymentDetails, financial.ORNumber, financial.PaymentDate,
                financial.AmountPaid, financial.Policy, financial.PolicyNumber,
                financial.PolicyID, financial.EffectivityStart, financial.Particular,
                financial.Building, financial.Content,
                financial.Premium, financial.TotalPremium, financial.Remarks,
                _principal.GetUserName(), financial.EffectivityEnd);
        }

        //pre-populate assetImages
        entity.Images = new List<AssetImageDocument>();
        foreach (AssetImageView imageView in Enum.GetValues(typeof(AssetImageView)))
        {
            var assetImageView = AssetImageDocument.Create(entity.Id, imageView, _principal.GetUserName());
            entity.Images.Add(assetImageView);
        }

        await _repository.Assets.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(entity.Id);
    }
}