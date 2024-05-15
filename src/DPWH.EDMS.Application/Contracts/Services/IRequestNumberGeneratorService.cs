namespace DPWH.EDMS.Application.Contracts.Services;

public interface IRequestNumberGeneratorService
{
    Task<string> Generate(DateTimeOffset currentYear, CancellationToken cancellationToken);
}
