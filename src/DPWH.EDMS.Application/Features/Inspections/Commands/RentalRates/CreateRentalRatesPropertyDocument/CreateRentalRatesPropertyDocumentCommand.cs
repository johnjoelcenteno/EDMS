using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesPropertyDocument;

public record CreateRentalRatesPropertyDocumentCommand(CreateRentalRatesPropertyDocumentRequest Request) : IRequest<SaveRentalRateDocumentResponse>;
internal sealed class CreateRentalRatesPropertyDocumentHandler : IRequestHandler<CreateRentalRatesPropertyDocumentCommand, SaveRentalRateDocumentResponse>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateRentalRatesPropertyDocumentHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<SaveRentalRateDocumentResponse> Handle(CreateRentalRatesPropertyDocumentCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        var rentalRatesProperty = _repository.RentalRateProperty.FirstOrDefault(i => i.Id == model.RentalRatesPropertyId);
        var rentalRatesPropertyDocument = _repository.RentalRatePropertyDocuments.FirstOrDefault(i => i.Id == model.Id);


        if (rentalRatesPropertyDocument is null)
        {
            rentalRatesPropertyDocument = RentalRatePropertyDocument.Create(model.Id ?? Guid.NewGuid(), rentalRatesProperty.Id, model.Name, model.Filename, model.Category, model.Uri, fileSize, _principal.GetUserName());
            _repository.RentalRatePropertyDocuments.Add(rentalRatesPropertyDocument);
        }
        else
        {
            rentalRatesPropertyDocument.Update(model.Filename, model.Name, model.Uri, fileSize, _principal.GetUserName());
        }
        await _repository.SaveChangesAsync(cancellationToken);

        return new SaveRentalRateDocumentResponse(rentalRatesPropertyDocument.Id);
    }
}
