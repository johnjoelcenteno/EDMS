using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Signatories.Queries
{
    public record GetSignatureRequest() : IRequest<string?>;
    public class GetSignature : IRequestHandler<GetSignatureRequest, string?>
    {
        private readonly IReadRepository _readRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public GetSignature(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal)
        {
            _readRepository = readRepository;
            _claimsPrincipal = claimsPrincipal;
        }
        public Task<string?> Handle(GetSignatureRequest request, CancellationToken cancellationToken)
        {
            var employeeNumber = _claimsPrincipal.GetEmployeeNumber();
            var uriSignature = _readRepository.UserProfileDocumentsView.FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
            if (uriSignature is null)
            {
                return null;
            }
            return Task.FromResult(uriSignature.UriSignature);
        }
    }
}
