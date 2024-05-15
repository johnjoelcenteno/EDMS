using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.UpdateConfigSetting;

public record UpdateConfigSettingCommand : IRequest<UpdateConfigSettingResult>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public string? Description { get; set; }
}

internal sealed class UpdateConfigSettingHandler : IRequestHandler<UpdateConfigSettingCommand, UpdateConfigSettingResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateConfigSettingHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateConfigSettingResult> Handle(UpdateConfigSettingCommand request, CancellationToken cancellationToken)
    {
        var configSetting = await _repository.ConfigSettings
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (configSetting is null)
        {
            throw new AppException($"Configuration with Id `{request.Id}` not found");
        }

        var duplicate = await _repository.ConfigSettings
            .FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);

        //if (duplicate is not null)
        //{
        //    throw new AppException($"Configuration with Name `{request.Name}` already exists");
        //}

        configSetting.UpdateDetails(request.Name, request.Value, request.Description, _principal.GetUserName());

        _repository.ConfigSettings.Update(configSetting);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateConfigSettingResult(configSetting);
    }
}