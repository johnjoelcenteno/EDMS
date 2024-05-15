using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibrary;

public record GetDataLibraryQuery : IRequest<IEnumerable<GetDataLibraryResult>>;

internal sealed class GetDataLibraryHandler : IRequestHandler<GetDataLibraryQuery, IEnumerable<GetDataLibraryResult>>
{
    private readonly IReadRepository _repository;

    public GetDataLibraryHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetDataLibraryResult>> Handle(GetDataLibraryQuery request, CancellationToken cancellationToken)
    {
        var library = await _repository.DataLibrariesView.ToListAsync(cancellationToken);

        return library.GroupBy(l => l.Type).Select(group => new GetDataLibraryResult
        {
            Type = group.Key,
            Data = group.Select(i => new GetDataLibraryResultValue(i)).ToArray()
        });
    }
}