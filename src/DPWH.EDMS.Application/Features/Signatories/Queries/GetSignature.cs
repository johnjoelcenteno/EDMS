using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.UserProfileDocuments;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Signatories.Queries
{
    public record GetSignatureRequest() : IRequest<GetUserProfileDocumentModel>;
    public class GetSignature : IRequestHandler<GetSignatureRequest, GetUserProfileDocumentModel>
    {
        private readonly IReadRepository _readRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public GetSignature(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal)
        {
            _readRepository = readRepository;
            _claimsPrincipal = claimsPrincipal;
        }
        public Task<GetUserProfileDocumentModel> Handle(GetSignatureRequest request, CancellationToken cancellationToken)
        {
            var employeeNumber = _claimsPrincipal.GetEmployeeNumber();
            var userProfileDocument = _readRepository.UserProfileDocumentsView.FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
            if (userProfileDocument is null)
            {
                return null;
            }

            GetUserProfileDocumentModel model = new()
            {
                Id = userProfileDocument.Id,
                EmployeeNumber = userProfileDocument.EmployeeNumber,
                UriSignature = userProfileDocument.UriSignature
            };

            return Task.FromResult(model);
        }
    }
}
