using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Assets.Commands;

public class SaveAssetFileResponse : IdResponse
{
    public SaveAssetFileResponse(Guid id) : base(id) { }
}

public class SaveFinancialFileResponse : IdResponse
{
    public SaveFinancialFileResponse(Guid id) : base(id) { }
}

public class SaveAssetFile : IRequest<SaveAssetFileResponse>
{
    public SaveAssetFile(SaveAssetFileRequest request)
    {
        Details = request;
    }

    [Required]
    public SaveAssetFileRequest Details { get; }

}

public class SaveFinancialFile : IRequest<SaveFinancialFileResponse>
{
    public SaveFinancialFile(SaveFinancialFileRequest request)
    {
        Details = request;
    }
    [Required]
    public SaveFinancialFileRequest Details { get; }
}
