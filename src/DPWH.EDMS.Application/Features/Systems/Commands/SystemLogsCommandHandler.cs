using System.Security.Claims;
using MediatR;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Systems.Commands;

public class SystemLogsCommandHandler : IRequestHandler<Create<CreateSystemLogsRequest, CreateResponse>, CreateResponse>,
                                           IRequestHandler<UpdateWithId<UpdateSystemLogsRequest, UpdateResponse>, UpdateResponse>,
                                           IRequestHandler<DeleteWithId<DeleteSystemLogsRequest, DeleteResponse>, DeleteResponse>

{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public SystemLogsCommandHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<CreateResponse> Handle(Create<CreateSystemLogsRequest, CreateResponse> request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var entity = SystemLog.Create(model.Version, model.Description, _principal.GetUserName(), model.Created);
        _writeRepository.SystemLogs.Add(entity);
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return new CreateResponse(entity.Id);
    }

    public async Task<UpdateResponse> Handle(UpdateWithId<UpdateSystemLogsRequest, UpdateResponse> request, CancellationToken cancellationToken)
    {
        var entity = await _writeRepository.SystemLogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity != null)
        {
            var model = request.Model;

            entity.UpdateDetails(model.Version, model.Description, _principal.GetUserName(), model.Created);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new UpdateResponse(entity.Id);
        }

        return new UpdateResponse(request.Id) { Success = false, Error = $"The request not found {request.Id}" };
    }

    public async Task<DeleteResponse> Handle(DeleteWithId<DeleteSystemLogsRequest, DeleteResponse> request, CancellationToken cancellationToken)
    {
        var entity = await _writeRepository.SystemLogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity != null)
        {
            _writeRepository.SystemLogs.Remove(entity);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new DeleteResponse(request.Id);
        }
        return new DeleteResponse(request.Id) { Success = false, Error = $"The request not found {request.Id}" };
    }
}
