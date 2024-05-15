using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Queries;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriInd;

public record GetPriIndQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetPriIndHandler : IRequestHandler<GetPriIndQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetPriIndHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetPriIndQuery request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyPriInd();
        var result = new DataSourceResult
        {
            Data = dummyData,
            Total = dummyData.Count
        };
        return Task.FromResult(result);
    }

    //TODO: Implement
    private static List<GetPriIndResult> CreateDummyPriInd()
    {
        return new List<GetPriIndResult>
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
                        Id = Guid.NewGuid(),
                        AssetId = Guid.NewGuid(),
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
                        Id = Guid.NewGuid(),
                        AssetId = Guid.NewGuid(),
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
    }
}
