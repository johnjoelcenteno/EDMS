using FluentValidation;

namespace DPWH.EDMS.Application.Features.Assets.Commands;

public class SaveAssetFileRequestValidator : AbstractValidator<SaveAssetFileRequest>
{
    public SaveAssetFileRequestValidator()
    {
        RuleFor(param => param.AssetId).NotEmpty();
        RuleFor(param => param.DocumentType).IsInEnum();
    }
}

public class AssetImageValidator : AbstractValidator<CreateAssetImageRequest>
{
    public AssetImageValidator()
    {
        RuleFor(param => param.AssetId).NotEmpty();
        RuleFor(param => param.File).NotNull();
        RuleFor(param => param.View).IsInEnum();
    }
}