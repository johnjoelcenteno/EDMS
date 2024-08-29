using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application;

public record class UpdateSignatoryRequest(Guid id, UpdateSignatoryModel model) : IRequest<Guid?>;
public class UpdateSignatory : IRequestHandler<UpdateSignatoryRequest, Guid?>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public UpdateSignatory(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _writeRepository = writeRepository;
        _claimsPrincipal = claimsPrincipal;
    }
    public async Task<Guid?> Handle(UpdateSignatoryRequest request, CancellationToken cancellationToken)
    {
        Signatory signatory = await _writeRepository.Signatories.FirstOrDefaultAsync(x => x.Id == request.id);
        if (signatory is null) return null;

        var model = request.model;
        var modifiedBy = _claimsPrincipal.GetUserName();
        signatory.UpdateDetails(
            model.DocumentType,
            model.Name,
            model.Position,
            model.Office1,
            model.Office2,
            model.SignatoryNo,
            _claimsPrincipal.GetUserName(),
            model.EmployeeNumber
        );

        if (model.IsActive)
        {
            signatory.Activate(modifiedBy);
        }
        else
        {
            signatory.Deactivate(modifiedBy);
        }

        await _writeRepository.SaveChangesAsync(cancellationToken);
        return signatory.Id;
    }
}
