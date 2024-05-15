using DPWH.EDMS.Domain.Entities;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument.File;

public record CreateRentalRateFileCommand(CreateRentalRatesDocumentRequest Request) : IRequest<SaveRentalRateDocumentResponse>;
public class CreateRentalRatesFileHandler : IRequestHandler<CreateRentalRateFileCommand, SaveRentalRateDocumentResponse>
{
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public CreateRentalRatesFileHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveRentalRateDocumentResponse> Handle(CreateRentalRateFileCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        //find the rental rate object 
        var rentalRate = _writeRepository.RentalRates.FirstOrDefault(i => i.Id == model.RentalRatesId);
        // for id file 
        var rentalRateFile = _writeRepository.RentalRatesFiles.FirstOrDefault(i => i.Id == model.Id);

        // create
        if (rentalRateFile is null)
        {
            rentalRateFile = RentalRateFileDocument.Create(model.Id ?? Guid.NewGuid(), rentalRate.Id, model.Name, model.Filename, model.Uri, fileSize, _principal.GetUserName());
            _writeRepository.RentalRatesFiles.Add(rentalRateFile);
        }
        else
        {
            // for update
            rentalRateFile.Update(model.Name, model.Filename, model.Uri, fileSize, _principal.GetUserName());
            _writeRepository.RentalRatesFiles.Update(rentalRateFile);
        }
        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveRentalRateDocumentResponse(rentalRateFile.Id);
    }
}
