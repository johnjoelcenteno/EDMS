using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application;

public record class DeleteSignatoryRequest(Guid Id) : IRequest<Guid?>;
public class DeleteSignatory : IRequestHandler<DeleteSignatoryRequest, Guid?>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public DeleteSignatory(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _writeRepository = writeRepository;
        _claimsPrincipal = claimsPrincipal;
    }

    public async Task<Guid?> Handle(DeleteSignatoryRequest request, CancellationToken cancellationToken)
    {
        Signatory signatory = _writeRepository.Signatories.FirstOrDefault(s => s.Id == request.Id);
        if (signatory is null) return null;

        signatory.Deactivate(_claimsPrincipal.GetUserName()); // SOFT DELETE
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return signatory.Id;
    }
}
