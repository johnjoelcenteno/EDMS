using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;

namespace DPWH.EDMS.Application.Services;

public interface ISequenceGeneratorService
{
    string GeneratePropertyId(Guid id);
}

public class SequenceGeneratorService : ISequenceGeneratorService
{
    private const int MaxRetryCount = 5;
    private readonly IReadRepository _repository;

    public SequenceGeneratorService(IReadRepository repository)
    {
        _repository = repository;
    }

    public string GeneratePropertyId(Guid id)
    {
        string propertyId = null;

        for (var i = 0; i < MaxRetryCount && propertyId == null; i++)
        {
            propertyId = ShortCodeGeneratorService.GenerateCode10(id, DateTimeOffset.Now);
            propertyId = string.Concat("PID", propertyId);
            if (_repository.AssetsView.Any(x => x.PropertyId == propertyId))
            {
                propertyId = null;
            }
        }

        SequenceGenerationFailException.ThrowIfNull(propertyId, nameof(propertyId));

        return propertyId;
    }
}