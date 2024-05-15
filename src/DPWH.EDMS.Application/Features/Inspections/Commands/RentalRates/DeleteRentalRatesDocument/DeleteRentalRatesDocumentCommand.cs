using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.DeleteRentalRatesDocument;

public record DeleteDocument(Guid Id) : IRequest<DeleteResponse>;
public class DeleteRentalRatesDocumentCommand : IRequestHandler<DeleteDocument, DeleteResponse>
{
    private readonly IWriteRepository _repository;

    public DeleteRentalRatesDocumentCommand(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteResponse> Handle(DeleteDocument request, CancellationToken cancellationToken)
    {
        var file = await _repository.RentalRatesFiles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (file is not null)
        {
            _repository.RentalRatesFiles.Remove(file);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        var image = await _repository.RentalRatesImages.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (image is not null)
        {
            _repository.RentalRatesImages.Remove(image);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        return new DeleteResponse(request.Id);
    }
}
