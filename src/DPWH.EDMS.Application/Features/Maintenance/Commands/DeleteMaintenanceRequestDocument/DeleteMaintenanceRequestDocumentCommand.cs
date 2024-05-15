using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.DeleteMaintenanceRequestDocument;

public record DeleteMaintenanceRequestDocumentCommand(Guid Id) : IRequest<DeleteResponse>;
public class DeleteMaintenanceRequestDocumentHandler : IRequestHandler<DeleteMaintenanceRequestDocumentCommand, DeleteResponse>
{
    private readonly IWriteRepository _repository;

    public DeleteMaintenanceRequestDocumentHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteResponse> Handle(DeleteMaintenanceRequestDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.MaintenanceRequestDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (document is not null)
        {
            _repository.MaintenanceRequestDocuments.Remove(document);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        return new DeleteResponse(request.Id);
    }
}
