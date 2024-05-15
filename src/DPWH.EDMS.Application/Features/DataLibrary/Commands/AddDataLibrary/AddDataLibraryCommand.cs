using System.Security.Claims;
using System.Text.Json.Serialization;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.AddDataLibrary;

public record AddDataLibraryCommand : IRequest<AddDataLibraryResult>
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required DataLibraryTypes Type { get; set; }
    public required string Value { get; set; }
}

internal sealed class AddDataLibraryHandler : IRequestHandler<AddDataLibraryCommand, AddDataLibraryResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public AddDataLibraryHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<AddDataLibraryResult> Handle(AddDataLibraryCommand request, CancellationToken cancellationToken)
    {
        var type = request.Type.ToString();
        var data = await _repository.DataLibraries
            .FirstOrDefaultAsync(d => d.Type == type && d.Value == request.Value, cancellationToken);

        if (data is not null)
        {
            throw new AppException($"Entry with Type `{type}` and Value `{data.Value}` already exists");
        }

        data = Domain.Entities.DataLibrary.Create(type, request.Value, _principal.GetUserName());
        await _repository.DataLibraries.AddAsync(data, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new AddDataLibraryResult(data);
    }
}