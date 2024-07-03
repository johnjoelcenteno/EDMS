using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibrary;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibraryById;

public record GetDataLibraryByIdQuery(Guid Id) : IRequest<GetDataLibraryResultValue?>;

internal sealed class GetDataLibraryByIdQueryHandler(IReadRepository repository) : IRequestHandler<GetDataLibraryByIdQuery, GetDataLibraryResultValue?>
{
    private readonly IReadRepository _repository = repository;

    public async Task<GetDataLibraryResultValue?> Handle(GetDataLibraryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.DataLibrariesView
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        return new GetDataLibraryResultValue(entity);
    }
}