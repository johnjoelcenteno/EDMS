using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Queries;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal2;

public record GetPriVal2Query(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetPriVal2Handler : IRequestHandler<GetPriVal2Query, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public GetPriVal2Handler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<DataSourceResult> Handle(GetPriVal2Query request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriVal2();
        var result = new DataSourceResult
        {
            Data = dummyData,
            Total = dummyData.Count
        };
        return Task.FromResult(result);
    }

    //TODO: Implement
    private static List<GetPriVal2Result> CreateDummyPriVal2()
    {
        var dummyData = new List<GetPriVal2Result>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                ProjectName = "Test Project Name",
                Location = "Metro Manila",
                Images =
                {
                    new AssetImageModel
                    {
                        Id = new Guid(),
                        AssetId = new Guid(),
                        Filename = "Test Image",
                        Category = "Image",
                        DocumentType = "Image",
                        Description = "Test Description",
                        Uri = "https://t3.ftcdn.net/jpg/02/49/54/22/240_F_249542271_pawMatSRDB8XkdWPxNi9F1nASXw9JhEy.jpg", // image from web
                        FileSize = 1000,
                        View = "Front"
                    },
                    new AssetImageModel
                    {
                        Id = new Guid(),
                        AssetId = new Guid(),
                        Filename = "Test Image",
                        Category = "Image",
                        DocumentType = "Image",
                        Description = "Test Description",
                        Uri = "https://t3.ftcdn.net/jpg/02/49/54/22/240_F_249542271_pawMatSRDB8XkdWPxNi9F1nASXw9JhEy.jpg", // image from web
                        FileSize = 1000,
                        View = "Back"
                    }
                }
            }
        };
        return dummyData;
    }
}
