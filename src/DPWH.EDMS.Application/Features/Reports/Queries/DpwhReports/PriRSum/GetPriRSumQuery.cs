using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriRSum;

public record GetPriRSumQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetPriRSumHandler : IRequestHandler<GetPriRSumQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetPriRSumHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetPriRSumQuery request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriRSum();
        var result = new DataSourceResult
        {
            Data = new List<object>
            {
                new
                {
                    PRIRSum = dummyData.PriRSum,
                    dummyData.TotalBuildings,
                    TotalEstimatedCostByImplementingOffice = dummyData.TotalEstimatedCostImplementingOffice,
                    TotalEstimatedCostByRegionalOffice = dummyData.TotalEstimatedCostRegionalOffice,

                }
            },
            Total = dummyData.PriRSum.Count
        };

        return Task.FromResult(result);
    }

    //TODO: Implement
    private static GetPriRSumResultTotal CreateDummyPriRSum()
    {
        var dummyData = new List<GetPriRSumResult>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                ProjectName = "Test Project Name",
                Location = "Metro Manila",
                NoOfBuildings = 2,
                EstimatedCostImplementingOffice = 20000,
                EstimatedCostRegionalOffice = 5000,
                POW = "Test Data POW",
                DUPA = "Test Data DUPA",
                DetailedEstimates = "Test Data DetailedEstimates",
                Plans = "Test Data Plans",
                NBSDNGOBPRIForm = "Test Data NBSDNGOBPRIForm",
                Remarks = "Test Data Remarks"
            },
            new()
            {
                Region = "Region II",
                ImplementingOffice = "Northern Luzon",
                ProjectName = "Another Project",
                Location = "Some Location",
                NoOfBuildings = 3,
                EstimatedCostImplementingOffice = 7000,
                EstimatedCostRegionalOffice = 80000,
                POW = "Another Test Data POW",
                DUPA = "Another Test Data DUPA",
                DetailedEstimates = "Another Test Data DetailedEstimates",
                Plans = "Another Test Data Plans",
                NBSDNGOBPRIForm = "Another Test Data NBSDNGOBPRIForm",
                Remarks = "Another Test Data Remarks"
            }
        };

        return new GetPriRSumResultTotal(dummyData);
    }
}
