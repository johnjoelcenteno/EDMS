using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriBomEval;

public record GetPribomEvalQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

public class GetPribomEvalHandler : IRequestHandler<GetPribomEvalQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetPribomEvalHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetPribomEvalQuery request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriBomEval();
        var result = new DataSourceResult
        {
            Data = new List<object>
            {
                new
                {
                    PRIBOMEval = dummyData.PriBomEval,
                    TotalRequestedAmount = dummyData.TotalRequestAmount,
                    dummyData.TotalEvaluatedAllocation

                }
            },
            Total = dummyData.PriBomEval.Count
        };

        return Task.FromResult(result);
    }

    //TODO: Implement
    private static GetPriBomEvalResultTotal CreateDummyPriBomEval()
    {
        var dummyData = new List<GetPriBomEvalResult>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                ProjectName = "Test Project Name",
                Location = "Metro Manila",
                RequestedAmount = 100000,
                EvaluatedAllocation = 500000,
                Inventory = "Another Test Data Inventory",
                ProofOfOwnership  = "Another test data proof of ownership",
                POW = "Test Data POW",
                DUPA = "Test Data DUPA",
                DetailedEstimates = "Test Data DetailedEstimates",
                Picture = "test Picture",
                Plan = "test Plan",
                YearFunded = "2023",
                CostOfRepair = "20000",
                Remarks = "Test Data Remarks"
            },
            new()
            {
                Region = "Region II",
                ImplementingOffice = "Northern Luzon",
                ProjectName = "Another Project",
                Location = "Some Location",
                RequestedAmount = 100000,
                EvaluatedAllocation = 500000,
                Inventory = "Another Test Data Inventory",
                ProofOfOwnership  = "Another test data proof of ownership",
                POW = "Another Test Data POW",
                DUPA = "Another Test Data DUPA",
                DetailedEstimates = "Another Test Data DetailedEstimates",
                Picture = "test Picture",
                Plan = "test Plan",
                YearFunded = "2023",
                CostOfRepair = "20000",
                Remarks = "Another Test Data Remarks"
            }
        };

        return new GetPriBomEvalResultTotal(dummyData);
    }
}