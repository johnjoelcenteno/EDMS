using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal1;

public record GetPriVal1Query(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetPriVal1Handler : IRequestHandler<GetPriVal1Query, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetPriVal1Handler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetPriVal1Query request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriVal1();
        var result = new DataSourceResult
        {
            Data = new List<object>
            {
                new
                {
                    PRIVal1 = dummyData.PriVal1,
                    TotalBuilding = dummyData.TotalBuildings,
                    TotalProposedMaintenance = dummyData.TotalProposedMaintenanceCost,
                    TotalEvaluatedMaintenance = dummyData.TotalEvaluatedMaintenanceCost
                }
            },
            Total = dummyData.PriVal1.Count
        };

        return Task.FromResult(result);
    }

    //TODO: Implement
    private static GetPriVal1ResultTotal CreateDummyPriVal1()
    {
        var dummyData = new List<GetPriVal1Result>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                ProjectName = "Test Project Name",
                Location = "Metro Manila",
                NoOfBuildings = 4,
                IncludedPriorityList = true,
                Validated = true,
                FundingRecommended = true,
                ProposedMaintenanceCost = 50000,
                EvaluatedMaintenanceCost = 40000,
                Remarks = "Test Remarks"
            },
            new()
            {
                Region = "Region II",
                ImplementingOffice = "Northern Luzon",
                ProjectName = "Another Project",
                Location = "Some Location",
                NoOfBuildings = 2,
                IncludedPriorityList = false,
                Validated = false,
                FundingRecommended = false,
                ProposedMaintenanceCost = 10000,
                EvaluatedMaintenanceCost = 4000,
                Remarks = "Test Remarks"

            }
        };

        return new GetPriVal1ResultTotal(dummyData);
    }
}
