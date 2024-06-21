namespace DPWH.EDMS.Application.Contracts.Services;

public interface IRequestNumberGeneratorService
{
    Task<string> Generate(DateTimeOffset currentYear, CancellationToken cancellationToken);
}

public interface IControlNumberGeneratorService
{
    Task<int> Generate(DateTimeOffset currentYear, CancellationToken cancellationToken);
}
