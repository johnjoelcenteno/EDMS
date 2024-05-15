using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Assets.Commands;

public class SaveAssetImageResponse : IdResponse
{
    public SaveAssetImageResponse(Guid id) : base(id) { }
}

public class SaveAssetImage : IRequest<SaveAssetImageResponse>
{
    public SaveAssetImage(SaveAssetImageRequest request)
    {
        Details = request;
    }

    [Required]
    public SaveAssetImageRequest Details { get; }

}
