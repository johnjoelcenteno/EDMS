using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.RecoverDataLibrary;

public record RecoverDataLibraryCommand(Guid Id) : IRequest<RecoverDataLibraryResult>;

internal sealed class RecoverDataLibraryHandler : IRequestHandler<RecoverDataLibraryCommand, RecoverDataLibraryResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public RecoverDataLibraryHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<RecoverDataLibraryResult> Handle(RecoverDataLibraryCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.DataLibraries.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (data is null)
        {
            throw new AppException("Data library entry does not exist");
        }

        if (!data.IsDeleted)
        {
            return new RecoverDataLibraryResult(data);
        }

        data.Undelete(_principal.GetUserName());

        _repository.DataLibraries.Update(data);
        await _repository.SaveChangesAsync(cancellationToken);

        return new RecoverDataLibraryResult(data);
    }
}