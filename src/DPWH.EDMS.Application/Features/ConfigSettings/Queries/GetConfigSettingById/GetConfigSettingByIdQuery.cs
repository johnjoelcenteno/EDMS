using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettingById;

public record GetConfigSettingByIdQuery(Guid Id) : IRequest<GetConfigSettingByIdResult>;

internal sealed class GetConfigSettingByIdHandler : IRequestHandler<GetConfigSettingByIdQuery, GetConfigSettingByIdResult>
{
    private readonly IReadRepository _repository;

    public GetConfigSettingByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetConfigSettingByIdResult> Handle(GetConfigSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var configSetting = await _repository.ConfigSettingsView
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (configSetting is null)
        {
            throw new AppException($"Configuration setting with Id `{request.Id}` not found");
        }

        return new GetConfigSettingByIdResult(configSetting);
    }
}