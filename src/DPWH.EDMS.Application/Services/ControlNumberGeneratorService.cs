using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Services;

public class ControlNumberGeneratorService : IControlNumberGeneratorService
{
    private readonly IWriteRepository _repository;
    private const string SequenceName = "ControlNumberSequence";

    public ControlNumberGeneratorService(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Generate(DateTimeOffset currentDate, CancellationToken cancellationToken)
    {
        var sequence = await GetNext(cancellationToken);
        var year = currentDate.Year;
        return int.Parse($"{year}{sequence}");
    }

    private async Task<string?> GetNext(CancellationToken cancellationToken)
    {
        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = $"SELECT NEXT VALUE FOR {SequenceName}";
        var sequence = (int)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence.ToString("D6");
    }

    private async Task<string?> ResetSequence(CancellationToken cancellationToken)
    {
        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = $"ALTER SEQUENCE {SequenceName} RESTART WITH 1";
        var sequence = (long?)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence?.ToString("D6");
    }

    private async Task<string?> GetCurrent(CancellationToken cancellationToken)
    {
        var sql = $"SELECT convert(int,current_value) FROM sys.sequences WHERE [name] = '{SequenceName}'";

        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = sql;
        var sequence = (long?)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence?.ToString("D6");
    }

    private async Task<string?> GetCurrentControlNumber(CancellationToken cancellationToken)
    {
        var sql = $"SELECT convert(int,current_value) FROM sys.sequences WHERE [name] = '{SequenceName}'";

        var connection = _repository.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = sql;
        var sequence = (long?)await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return sequence?.ToString("D6");
    }
}
