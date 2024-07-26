using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using System.Linq.Dynamic.Core;

namespace DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibrary;

public record GetDataLibraryQuery(DataSourceRequest request) : IRequest<DataSourceResult>;

internal sealed class GetDataLibraryHandler : IRequestHandler<GetDataLibraryQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetDataLibraryHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<DataSourceResult> Handle(GetDataLibraryQuery request, CancellationToken cancellationToken)
    {
        var library = _repository.DataLibrariesView;
        var dataLibRequest = library.OrderByDescending(x => x.Created)
            .Select(data => new GetAllDataLibraray
            {
                Id = data.Id,
                Value = data.Value,
                Type = data.Type,
                IsDeleted = data.IsDeleted,
                Created = data.Created,
                CreatedBy = data.CreatedBy
            })
            .ToDataSourceResult(request.request.FixSerialization());

        return await Task.FromResult(dataLibRequest);
    }
}   