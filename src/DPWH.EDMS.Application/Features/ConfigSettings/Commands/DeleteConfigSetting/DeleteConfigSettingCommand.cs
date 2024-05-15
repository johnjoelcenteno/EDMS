using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.DeleteConfigSetting;

public record DeleteConfigSettingCommand(Guid Id) : IRequest<DeleteConfigSettingResult>;

internal sealed class DeleteConfigSettingHandler : IRequestHandler<DeleteConfigSettingCommand, DeleteConfigSettingResult>
{
    private readonly IWriteRepository _repository;

    public DeleteConfigSettingHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteConfigSettingResult> Handle(DeleteConfigSettingCommand request, CancellationToken cancellationToken)
    {
        var configSetting = await _repository.ConfigSettings
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (configSetting is null)
        {
            throw new AppException($"Configuration setting `{request.Id}` not found");
        }

        _repository.ConfigSettings.Remove(configSetting);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteConfigSettingResult(configSetting);
    }
}