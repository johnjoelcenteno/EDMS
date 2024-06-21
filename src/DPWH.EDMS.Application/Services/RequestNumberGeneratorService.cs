using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Services;

public class RequestNumberGeneratorService : IRequestNumberGeneratorService
{
    private readonly IWriteRepository _repository;
    private const string SequenceName = "MaintenanceRequestNumberSequence";

    public RequestNumberGeneratorService(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Generate(DateTimeOffset currentDate, CancellationToken cancellationToken)
    {
        var sequence = await GetNext(cancellationToken);
        var year = currentDate.Year;

        return $"MR-{year}-{sequence}";
    }

    private async Task<string?> GetNext(CancellationToken cancellationToken)
    {
        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = $"SELECT NEXT VALUE FOR {SequenceName}";
        var sequence = (long?)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence?.ToString("D5");
    }
}
