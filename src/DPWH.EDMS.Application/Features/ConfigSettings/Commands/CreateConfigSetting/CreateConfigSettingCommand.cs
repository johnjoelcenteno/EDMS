using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.CreateConfigSetting;

public record CreateConfigSettingCommand(string Name, string Value, string? Description) : IRequest<CreateConfigSettingResult>;

internal sealed class CreateConfigSettingHandler : IRequestHandler<CreateConfigSettingCommand, CreateConfigSettingResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateConfigSettingHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<CreateConfigSettingResult> Handle(CreateConfigSettingCommand request, CancellationToken cancellationToken)
    {
        var configSetting = await _repository.ConfigSettings
            .FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);

        if (configSetting is not null)
        {
            throw new AppException($"Configuration `{request.Name}` already exists");
        }

        configSetting = ConfigSetting.Create(request.Name, request.Value, request.Description, _principal.GetUserName());

        await _repository.ConfigSettings.AddAsync(configSetting, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateConfigSettingResult(configSetting);
    }
}