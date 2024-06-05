using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application;

public record DocumentQueryRequest(DataSourceRequest request) : IRequest<DataSourceResult>;
public class QueryDocumentRequest : IRequestHandler<DocumentQueryRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QueryDocumentRequest(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    public async Task<DataSourceResult> Handle(DocumentQueryRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository.DocumentRequestView.OrderByDescending(x => x.Created).ToDataSourceResult(request.request.FixSerialization());
        return result;
    }
}
