using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.Signatories.Queries;

public record GetSignatoryByEmployeeId(string EmployeeId) : IRequest<QuerySignatoryModel>;
public class GetSignatoryByEmployeeIdHandler : IRequestHandler<GetSignatoryByEmployeeId, QuerySignatoryModel>
{
    private readonly IReadRepository _readRepository;

    public GetSignatoryByEmployeeIdHandler(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal)
    {
        _readRepository = readRepository;
    }

    public Task<QuerySignatoryModel> Handle(GetSignatoryByEmployeeId request, CancellationToken cancellationToken)
    {
        var userProfileDocument = _readRepository.SignatoriesView.FirstOrDefault(x => x.EmployeeNumber == request.EmployeeId);

        if (userProfileDocument is null)
        {
            return Task.FromResult<QuerySignatoryModel>(null);
        }

        QuerySignatoryModel model = new()
        {
            Id = userProfileDocument.Id,
            DocumentType = userProfileDocument.DocumentType,
            Name = userProfileDocument.Name,
            Position = userProfileDocument.Position,
            Office1 = userProfileDocument.Office1,
            Office2 = userProfileDocument.Office2,
            SignatoryNo = userProfileDocument.SignatoryNo,
            IsActive = userProfileDocument.IsActive,
            EmployeeNumber = userProfileDocument.EmployeeNumber

        };

        return Task.FromResult(model);
    }
}