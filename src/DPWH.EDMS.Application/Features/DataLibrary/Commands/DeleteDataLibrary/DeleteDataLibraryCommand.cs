using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.DeleteDataLibrary;

public record DeleteDataLibraryCommand(Guid Id) : IRequest<DeleteDataLibraryResult>;

internal sealed class DeleteDataLibraryHandler : IRequestHandler<DeleteDataLibraryCommand, DeleteDataLibraryResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public DeleteDataLibraryHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<DeleteDataLibraryResult> Handle(DeleteDataLibraryCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.DataLibraries.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (data is null)
        {
            throw new AppException("Data library entry does not exist");
        }

        if (data.IsDeleted)
        {
            return new DeleteDataLibraryResult(data);
        }

        data.Delete(_principal.GetUserName());
        _repository.DataLibraries.Update(data);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteDataLibraryResult(data);
    }
}