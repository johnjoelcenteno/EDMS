using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesProperty;

public record CreateRentalRatesPropertyCommand : IRequest<Guid>
{
    public string PropertyName { get; set; }
    public string? TypeOfStructure { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? Group { get; set; }
    public string? Agency { get; set; }
    public string? AttachedAgency { get; set; }
    public string? Region { get; set; }
    public string? RegionId { get; set; }
    public string? Province { get; set; }
    public string? ProvinceId { get; set; }
    public string? City { get; set; }
    public string? CityId { get; set; }
    //public string? Barangay { get; set; }
    //public string? BarangayId { get; set; }
    public string? StreetAddress { get; set; }
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
}
internal sealed class CreateRentalRatesPropertyHandler : IRequestHandler<CreateRentalRatesPropertyCommand, Guid>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly IRentalRateNumberGeneratorService _generatorService;


    public CreateRentalRatesPropertyHandler(IWriteRepository repository, ClaimsPrincipal principal, IRentalRateNumberGeneratorService generatorService)
    {
        _repository = repository;
        _principal = principal;
        _generatorService = generatorService;
    }

    public async Task<Guid> Handle(CreateRentalRatesPropertyCommand request, CancellationToken cancellationToken)
    {
        var currentYear = DateTimeOffset.Now;
        var rentalRateNumber = await _generatorService.Generate(currentYear, cancellationToken);

        var entity = RentalRateProperty.Create(rentalRateNumber, request.PropertyName, request.TypeOfStructure, request.ImplementingOffice, request.Group, request.Agency, request.AttachedAgency, request.Region, request.RegionId, request.Province, request.ProvinceId, request.City, request.CityId, request.StreetAddress, request.LongDegrees, request.LongMinutes, request.LongSeconds, request.LongDirection, request.LatDegrees, request.LatMinutes, request.LatSeconds, request.LatDirection, _principal.GetUserName());

        await _repository.RentalRateProperty.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
