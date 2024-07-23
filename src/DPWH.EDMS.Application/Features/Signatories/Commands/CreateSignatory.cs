using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application;

public record class CreateSignatoryRequest(CreateSignatoryModel model) : IRequest<Guid>;
public class CreateSignatory : IRequestHandler<CreateSignatoryRequest, Guid>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public CreateSignatory(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _writeRepository = writeRepository;
        _claimsPrincipal = claimsPrincipal;
    }
    public async Task<Guid> Handle(CreateSignatoryRequest request, CancellationToken cancellationToken)
    {
        var model = request.model;
        var creator = _claimsPrincipal.GetUserName();
        Signatory signatory = Signatory.Create(
            model.DocumentType,
            model.Name,
            model.Position,
            model.Office1,
            creator,
            model.Office2,
            request.model.SignatoryNo
        );

        if (model.IsActive)
        {
            signatory.Activate(creator);
        }
        else
        {
            signatory.Deactivate(creator);
        }

        await _writeRepository.Signatories.AddAsync(signatory, cancellationToken);
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return signatory.Id;
    }
}
