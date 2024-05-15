using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.UpdateDataLibrary;

public record UpdateDataLibraryCommand(Guid Id, string Value) : IRequest<UpdateDataLibraryResult>;

internal sealed class UpdateDataLibraryHandler : IRequestHandler<UpdateDataLibraryCommand, UpdateDataLibraryResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateDataLibraryHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateDataLibraryResult> Handle(UpdateDataLibraryCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.DataLibraries
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (data is null)
        {
            throw new AppException("Data Library entry does not exist");
        }

        if (data.Value.Equals(request.Value, StringComparison.InvariantCulture))
        {
            return new UpdateDataLibraryResult(data, data.Value);
        }

        var oldValue = data.Value;

        data.Update(request.Value, _principal.GetUserName());
        _repository.DataLibraries.Update(data);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateDataLibraryResult(data, oldValue);
    }
}