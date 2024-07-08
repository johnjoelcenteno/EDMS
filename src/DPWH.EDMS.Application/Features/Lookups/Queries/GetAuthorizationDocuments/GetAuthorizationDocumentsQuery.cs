using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.DataLibrary;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetAuthorizationDocuments;
public record GetAuthorizationDocumentsQuery : IRequest<IEnumerable<GetAuthorizationDocumentsResult>>;

internal sealed class GetGetAuthorizationDocumentsQueryHandler(IReadRepository repository) : IRequestHandler<GetAuthorizationDocumentsQuery, IEnumerable<GetAuthorizationDocumentsResult>>
{
    public async Task<IEnumerable<GetAuthorizationDocumentsResult>> Handle(GetAuthorizationDocumentsQuery request, CancellationToken cancellationToken)
    {
        var recordTypes = await repository.DataLibrariesView
            .Where(d => d.Type == DataLibraryTypes.AuthorizationDocuments.ToString() && !d.IsDeleted)
            .Select(d => new GetAuthorizationDocumentsResult(d.Id, d.Value))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}