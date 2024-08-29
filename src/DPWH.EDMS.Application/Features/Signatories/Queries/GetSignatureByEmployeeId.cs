using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.UserProfileDocuments;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Signatories.Queries;

public record GetSignatureByEmployeeId(string EmployeeId) : IRequest<GetUserProfileDocumentModel>;
public class GetSignatureByEmployeeIdHandler : IRequestHandler<GetSignatureByEmployeeId, GetUserProfileDocumentModel>
{
    private readonly IReadRepository _readRepository;
   
    public GetSignatureByEmployeeIdHandler(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal)
    {
        _readRepository = readRepository;
    }

    public Task<GetUserProfileDocumentModel> Handle(GetSignatureByEmployeeId request, CancellationToken cancellationToken)
    {
        var userProfileDocument = _readRepository.UserProfileDocumentsView.FirstOrDefault(x => x.EmployeeNumber == request.EmployeeId);

        if (userProfileDocument is null)
        {
            return Task.FromResult<GetUserProfileDocumentModel>(null);
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