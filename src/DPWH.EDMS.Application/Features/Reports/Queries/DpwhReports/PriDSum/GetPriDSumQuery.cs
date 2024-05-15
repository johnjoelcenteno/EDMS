using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriDSum;

public record GetPriDSumQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetPriDSumHandler : IRequestHandler<GetPriDSumQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetPriDSumHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetPriDSumQuery request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriDSum();
        var result = new DataSourceResult
        {
            Data = new List<object>
            {
                new
                {
                    PRIDSum = dummyData.PriDSum,
                    dummyData.TotalBuildings,
                    dummyData.TotalCost
                }
            },
            Total = dummyData.PriDSum.Count
        };

        return Task.FromResult(result);
    }

    //TODO: Implement
    private static GetPriDSumResultTotal CreateDummyPriDSum()
    {
        var dummyData = new List<GetPriDSumResult>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                ProjectName = "Test Project Name",
                Location = "Metro Manila",
                NoOfBuildings = 2,
                EstimatedCost = 20000,
                ScopeOfWork = "Test Scope"
            },
            new()
            {
                Region = "Region II",
                ImplementingOffice = "Northern Luzon",
                ProjectName = "Another Project",
                Location = "Some Location",
                NoOfBuildings = 3,
                EstimatedCost = 30000,
                ScopeOfWork = "Another Scope",
            }
        };

        return new GetPriDSumResultTotal(dummyData);
    }
}
