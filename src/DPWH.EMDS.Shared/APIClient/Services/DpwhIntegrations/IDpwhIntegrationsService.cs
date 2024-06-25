using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;

public interface IDpwhIntegrationsService
{
    Task<EmployeeBaseApiResponse> GetByEmployeeId(string id);
}

