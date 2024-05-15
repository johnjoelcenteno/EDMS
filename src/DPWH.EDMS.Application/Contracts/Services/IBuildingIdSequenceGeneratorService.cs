namespace DPWH.EDMS.Application.Contracts.Services;

public interface IBuildingIdSequenceGeneratorService
{
    Task<string> Generate(string? departmentCode, string? requestingOffice, CancellationToken cancellationToken);
}