using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.DeleteProjectMonitoringDocument;

public record DeleteProjectMonitoringDocumentCommand(Guid Id) : IRequest<DeleteResponse>;
internal sealed class DeleteProjectMonitoringDocumentHandler : IRequestHandler<DeleteProjectMonitoringDocumentCommand, DeleteResponse>
{
    private readonly IWriteRepository _repository;

    public DeleteProjectMonitoringDocumentHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteResponse> Handle(DeleteProjectMonitoringDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.ProjectMonitoringDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (document is not null)
        {
            _repository.ProjectMonitoringDocuments.Remove(document);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        return new DeleteResponse(request.Id);
    }
}
