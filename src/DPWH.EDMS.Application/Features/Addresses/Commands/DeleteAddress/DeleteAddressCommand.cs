using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(string Id) : IRequest<DeleteAddressResult>;

internal sealed class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, DeleteAddressResult>
{
    private readonly IWriteRepository _repository;

    public DeleteAddressHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteAddressResult> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _repository.Geolocations.FirstOrDefaultAsync(a => a.MyId == request.Id, cancellationToken);

        if (address is null)
        {
            throw new AppException($"Address with Id `{request.Id}` not found.");
        }

        _repository.Geolocations.Remove(address);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteAddressResult(address);
    }
}