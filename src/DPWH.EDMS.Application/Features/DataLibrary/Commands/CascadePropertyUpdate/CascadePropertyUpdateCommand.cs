using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.UpdateDataLibrary;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.CascadePropertyUpdate;

public record CascadePropertyUpdateCommand(UpdateDataLibraryResult UpdateDataLibraryResult) : IRequest;

internal sealed class CascadePropertyUpdateHandler : IRequestHandler<CascadePropertyUpdateCommand>
{
    private readonly IWriteRepository _repository;

    public CascadePropertyUpdateHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CascadePropertyUpdateCommand request, CancellationToken cancellationToken)
    {
        var dataLibraryType = request.UpdateDataLibraryResult.Type;
        var oldValue = request.UpdateDataLibraryResult.OldValue;
        var newValue = request.UpdateDataLibraryResult.Value;

        switch (dataLibraryType)
        {            
            case DataLibraryTypes.Purposes:
                await _repository.RecordRequests.Where(a => a.Purpose == oldValue)
                    .ExecuteUpdateAsync(s => s.SetProperty(a => a.Purpose, a=>newValue),cancellationToken);
                break;
            case DataLibraryTypes.ValidIDs:
                await _repository.RecordRequestDocuments.Where(a => a.Name == oldValue && a.Type == "ValidId").ExecuteUpdateAsync(s => s.SetProperty(a => a.Name,a=>newValue),cancellationToken);
                break;
            case DataLibraryTypes.AuthorizationDocuments:
                await _repository.RecordRequestDocuments.Where(a => a.Name == oldValue && a.Type == "AuthorizationDocument").ExecuteUpdateAsync(s => s.SetProperty(a => a.Name, a => newValue), cancellationToken);
                break;

            default: throw new AppException($"Update propagation for {dataLibraryType} is not yet supported");
        }
    }
}