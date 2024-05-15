using System.Security.Claims;
using System.Text.Json.Serialization;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.CreateAddress;

public record CreateAddressCommand : IRequest<CreateAddressResult>
{
    public string Id { get; set; }
    public required string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required GeoLocationTypes Type { get; set; }
    public string? ParentId { get; set; }
}

internal sealed class CreateAddressHandler : IRequestHandler<CreateAddressCommand, CreateAddressResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateAddressHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<CreateAddressResult> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = GeoLocation.Create(request.Id, null, request.Name, request.Type.ToString(), request.ParentId, _principal.GetUserName());

        await _repository.GeoLocation.AddAsync(address, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateAddressResult(address);
    }
}