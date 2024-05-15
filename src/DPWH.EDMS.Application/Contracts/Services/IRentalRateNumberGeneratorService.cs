namespace DPWH.EDMS.Application.Contracts.Services;

public interface IRentalRateNumberGeneratorService
{
    Task<string> Generate(DateTimeOffset currentYear, CancellationToken cancellationToken);
}
