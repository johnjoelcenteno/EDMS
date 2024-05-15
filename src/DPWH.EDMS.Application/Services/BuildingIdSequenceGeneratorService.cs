using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Services;

public class BuildingIdSequenceGeneratorService : IBuildingIdSequenceGeneratorService
{
    private readonly IWriteRepository _repository;
    private const string SequenceName = "BuildingIdSequence";

    public BuildingIdSequenceGeneratorService(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Generate(string? departmentCode, string? requestingOffice, CancellationToken cancellationToken)
    {
        var agency = await GetAgency(departmentCode, cancellationToken);
        var office = await GetRequestingOffice(requestingOffice, cancellationToken);
        var sequence = await GetNext(cancellationToken);

        return $"N{agency.AgencyNumberCode}{office.NumberCode}{sequence}";
    }

    private async Task<string?> GetNext(CancellationToken cancellationToken)
    {
        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = $"SELECT NEXT VALUE FOR {SequenceName}";
        var sequence = (long?)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence?.ToString("D4");
    }

    private async Task<Agency> GetAgency(string? agencyCode, CancellationToken cancellationToken)
    {
        var agency = await _repository.Agencies
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AgencyCode == agencyCode, cancellationToken);

        if (agency is null)
        {
            throw new AppException($"Agency `{agencyCode}` not found");
        }

        return agency;
    }

    private async Task<RequestingOffice> GetRequestingOffice(string? officeName, CancellationToken cancellationToken)
    {
        var office = await _repository.RequestingOffices
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name == officeName, cancellationToken);

        if (office is null)
        {
            throw new AppException($"Requesting/Regional Office `{officeName}` not found");
        }

        return office;
    }
}