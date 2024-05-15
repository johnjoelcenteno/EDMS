using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesProperty;
using DPWH.EDMS.Application.Features.Assets.Queries;
using DPWH.EDMS.Domain.Entities;
using System.Linq.Expressions;

namespace DPWH.EDMS.Application.Features.Inspections.Mappers;

public static class RentalRatesPropertyMappers
{
    public static Expression<Func<RentalRateProperty, RentalRatesPropertyModel>> MapToModelExpression()
    {
        return entity => new RentalRatesPropertyModel
        {
            Id = entity.Id,
            RentalRateNumber = entity.RentalRateNumber,
            PropertyName = entity.PropertyName,
            TypeOfStructure = entity.TypeOfStructure,
            ImplementingOffice = entity.ImplementingOffice,
            Group = entity.Group,
            Agency = entity.Agency,
            AttachedAgency = entity.AttachedAgency,
            Region = entity.Region,
            Province = entity.Province,
            CityOrMunicipalityId = entity.CityOrMunicipality,
            Barangay = entity.Barangay,
            StreetAddress = entity.StreetAddress,
            Longitude = new LongLatFormat
            {
                Degrees = entity.LongDegrees,
                Minutes = entity.LongMinutes,
                Seconds = entity.LongSeconds,
                Direction = entity.LongDirection
            },
            Latitude = new LongLatFormat
            {
                Degrees = entity.LatDegrees,
                Minutes = entity.LatMinutes,
                Seconds = entity.LatSeconds,
                Direction = entity.LatDirection
            }
        };
    }
}
