using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Licenses
{
    public interface ILicensesService
    {
        Task<GetLicenseStatusResultBaseApiResponse> GetLicenseStatus();
    }
}