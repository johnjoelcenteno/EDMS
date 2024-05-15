using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Commands;

public class AssetDocumentsCommandHandler : IRequestHandler<Create<CreateAssetImageRequest, CreateResponse>, CreateResponse>,
                                            IRequestHandler<SaveAssetImage, SaveAssetImageResponse>,
                                            IRequestHandler<SaveAssetFile, SaveAssetFileResponse>,
                                            IRequestHandler<DeleteWithId<DeleteAssetDocumentRequest, DeleteResponse>, DeleteResponse>,
                                            IRequestHandler<SaveFinancialFile, SaveFinancialFileResponse>
{

    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;

    public AssetDocumentsCommandHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _writeRepository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveAssetImageResponse> Handle(SaveAssetImage request, CancellationToken cancellationToken)
    {
        var model = request.Details;
        long fileSize = model.File.Length;

        var assetImage = _writeRepository.AssetImages.FirstOrDefault(i => i.Id == model.Id);
        if (assetImage is null)
        {
            assetImage = AssetImageDocument.Create(model.Id, model.AssetId, model.Filename, model.View, model.Description, model.Uri, model.LongDegrees, model.LongMinutes, model.LongSeconds, model.LongDirection, model.LatDegrees, model.LatMinutes, model.LatSeconds, model.LatDirection, fileSize, _principal.GetUserName());
            _writeRepository.AssetImages.Add(assetImage);
        }
        else
        {
            assetImage.Update(model.Filename, model.Description, model.View, model.Uri, model.LongDegrees, model.LongMinutes, model.LongSeconds, model.LongDirection, model.LatDegrees, model.LatMinutes, model.LatSeconds, model.LatDirection, fileSize, _principal.GetUserName());
        }

        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveAssetImageResponse(assetImage.Id);
    }
    public async Task<SaveAssetFileResponse> Handle(SaveAssetFile request, CancellationToken cancellationToken)
    {
        var model = request.Details;
        long? fileSize = model.File?.Length;
        string? fileName = model.Filename;
        string? uri = model.Uri;

        var assetDocument = _writeRepository.AssetFiles.FirstOrDefault(i => i.Id == model.Id);
        if (assetDocument is null)
        {
            assetDocument = AssetFileDocument.Create(model.Id, model.AssetId, model.Filename, model.DocumentType, model.DocumentNo, model.DocumentTypeOthers, model.OtherRelatedDocuments, model.Description, model.Uri, fileSize ?? 0, _principal.GetUserName());
            _writeRepository.AssetFiles.Add(assetDocument);
        }
        else
        {
            assetDocument.Update(fileName ?? assetDocument.Filename, model.DocumentType, model.DocumentNo, model.DocumentTypeOthers, model.OtherRelatedDocuments ?? assetDocument.OtherRelatedDocuments, model.Description, uri ?? assetDocument.Uri, fileSize ?? assetDocument.FileSize, _principal.GetUserName());
        }

        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new SaveAssetFileResponse(assetDocument.Id);
    }
    public async Task<SaveFinancialFileResponse> Handle(SaveFinancialFile request, CancellationToken cancellationToken)
    {
        var model = request.Details;
        long? fileSize = model.File?.Length;

        var assetDocument = _writeRepository.FinancialDetailsDocuments.FirstOrDefault(doc => doc.Id == model.Id);
        if (assetDocument is null)
        {
            assetDocument = FinancialDetailsDocuments.Create(model.Id, model.AssetId, model.YearFunded, model.Allocation, model.FileName, model.Uri, fileSize ?? 0, _principal.GetUserName());
            _writeRepository.FinancialDetailsDocuments.Add(assetDocument);
        }
        else
        {
            assetDocument.Update(model.YearFunded ?? assetDocument.YearFunded, model.Allocation ?? assetDocument.Allocation, model.FileName ?? assetDocument.FileName, model.Uri ?? assetDocument.Uri,
                                 model.FileSize ?? assetDocument.FileSize, _principal.GetUserName());
        }

        await _writeRepository.SaveChangesAsync(cancellationToken);
        return new SaveFinancialFileResponse(assetDocument.Id);
    }
    /// <summary>
    /// Obsolete
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateResponse> Handle(Create<CreateAssetImageRequest, CreateResponse> request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        long fileSize = model.File.Length;
        var entity = AssetImageDocument.Create(Guid.NewGuid(), model.AssetId, model.Filename, model.View, model.Description, model.Uri, model.LongDegrees, model.LongMinutes, model.LongSeconds, model.LongDirection, model.LatDegrees, model.LatMinutes, model.LatSeconds, model.LatDirection, fileSize, _principal.GetUserName());
        _writeRepository.AssetImages.Add(entity);
        await _writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(entity.Id);
    }

    public async Task<DeleteResponse> Handle(DeleteWithId<DeleteAssetDocumentRequest, DeleteResponse> request, CancellationToken cancellationToken)
    {
        var assetImage = await _writeRepository.AssetImages.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (assetImage is not null)
        {
            _writeRepository.AssetImages.Remove(assetImage);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new DeleteResponse(request.Id);
        }

        var assetFile = await _writeRepository.AssetFiles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (assetFile is not null)
        {
            _writeRepository.AssetFiles.Remove(assetFile);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new DeleteResponse(request.Id);
        }

        var financialFile = await _writeRepository.FinancialDetailsDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (financialFile is not null)
        {
            _writeRepository.FinancialDetailsDocuments.Remove(financialFile);
            await _writeRepository.SaveChangesAsync(cancellationToken);
            return new DeleteResponse(request.Id);
        }
        return new DeleteResponse(request.Id) { Success = false, Error = $"The request not found {request.Id}" };
    }

}
