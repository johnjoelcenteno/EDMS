using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application;

public record class QuerySignatoryRequest(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;
public class QuerySignatory : IRequestHandler<QuerySignatoryRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QuerySignatory(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    public Task<DataSourceResult> Handle(QuerySignatoryRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository
                .SignatoriesView
                .Select(x => new QuerySignatoryModel() // Fix for where "could not be translated error"
                {
                    Id = x.Id,
                    DocumentType = x.DocumentType,
                    Name = x.Name,
                    Position = x.Position,
                    Office1 = x.Office1,
                    Office2 = x.Office2,
                    SignatoryNo = x.SignatoryNo,
                    IsActive = x.IsActive,
                    EmployeeNumber = x.EmployeeNumber
                })
                .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}
