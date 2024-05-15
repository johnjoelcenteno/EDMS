using DPWH.EDMS.Domain.Entities;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument.Image;

public record CreateRentalRatesImageCommand(CreateRentalRatesDocumentRequest Request) : IRequest<SaveRentalRateDocumentResponse>;
public class CreateRentalRatesImageHandler : IRequestHandler<CreateRentalRatesImageCommand, SaveRentalRateDocumentResponse>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public CreateRentalRatesImageHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveRentalRateDocumentResponse> Handle(CreateRentalRatesImageCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        var rentalRate = _writeRepository.RentalRates.FirstOrDefault(i => i.Id == model.RentalRatesId);
        var rentalRateImage = _writeRepository.RentalRatesImages.FirstOrDefault(i => i.Id == model.RentalRatesId);

        if (rentalRateImage is null)
        {
            rentalRateImage = RentalRateImageDocument.Create(model.Id ?? Guid.NewGuid(), rentalRate.Id, model.Name, model.Filename, model.Group, model.Category, model.Uri, fileSize, _principal.GetUserName());
            _writeRepository.RentalRatesImages.Add(rentalRateImage);
        }
        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveRentalRateDocumentResponse(rentalRateImage.Id);
    }
}
