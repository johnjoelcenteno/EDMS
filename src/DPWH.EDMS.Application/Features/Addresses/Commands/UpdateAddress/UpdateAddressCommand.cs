using System.Security.Claims;
using System.Text.Json.Serialization;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand : IRequest<UpdateAddressResult>
{
    public string Id { get; set; }
    public required string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required GeoLocationTypes Type { get; set; }
    public string? ParentId { get; set; }
}

internal sealed class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, UpdateAddressResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateAddressHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }
    public async Task<UpdateAddressResult> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _repository.GeoLocation
            .FirstOrDefaultAsync(a => a.MyId == request.Id, cancellationToken);

        if (address is null)
        {
            throw new AppException($"Address with Id `{request.Id}` not found.");
        }

        address.UpdateDetails(address.MyId, request.Id, request.Name, request.Type.ToString(), request.ParentId, _principal.GetUserName());
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateAddressResult(address);
    }
}