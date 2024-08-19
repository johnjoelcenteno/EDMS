using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.Signatories.Commands
{
    public record UpsertSignatoryUriRequest(string Uri) : IRequest<Guid>;
    public class UpsertSignatoryUri : IRequestHandler<UpsertSignatoryUriRequest, Guid>
    {
        private readonly IWriteRepository _writeRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UpsertSignatoryUri(IWriteRepository writeRepository, ClaimsPrincipal principal)
        {
            _writeRepository = writeRepository;
            _claimsPrincipal = principal;
        }
        public async Task<Guid> Handle(UpsertSignatoryUriRequest request, CancellationToken cancellationToken)
        {
            var employeeNumber = _claimsPrincipal.GetEmployeeNumber();
            var employeeUsername = _claimsPrincipal.GetUserName();
            Guid id;

            UserProfileDocument userProfileDocument = _writeRepository.UserProfileDocuments.FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
            
            // Implement upsert
            if (userProfileDocument is null)
            {
                var newProfileDocument = new UserProfileDocument(employeeNumber, request.Uri, employeeUsername);
                id = newProfileDocument.Id;

                _writeRepository.UserProfileDocuments.Add(newProfileDocument);
            }
            else
            {
                userProfileDocument.Update(request.Uri, employeeUsername);
                id = userProfileDocument.Id;
            }

            await _writeRepository.SaveChangesAsync();
            return id;
        }
    }
}
