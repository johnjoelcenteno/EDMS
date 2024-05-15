namespace DPWH.EDMS.Application.Contracts.Services;

public interface IDpwhApiService
{
    Task<T?> Get<T>(string operation, string? employeeId = null);
    Task<T?> GetWithRetry<T>(string operation, string? employeeId = null);

    Task<string> GetRaw(string operation, string? employeeId = null);
    Task<T?> GetLocation<T>(string operation, string? type = null, string? id = null);
    Task<T?> GetLocationWithRetry<T>(string operation, string? type = null, string? id = null);
}
