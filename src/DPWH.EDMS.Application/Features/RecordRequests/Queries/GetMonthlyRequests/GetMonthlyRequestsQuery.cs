using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests;

public record GetMonthlyRequestQuery() : IRequest<List<GetMonthlyRequestModel>>;
internal sealed class GetMonthlyRequestsHandler(IReadRepository readRepository) : IRequestHandler<GetMonthlyRequestQuery, List<GetMonthlyRequestModel>>
{
    private readonly IReadRepository _readRepository = readRepository;
    private static readonly string[] MonthNames =
        [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" ];

    public async Task<List<GetMonthlyRequestModel>> Handle(GetMonthlyRequestQuery request, CancellationToken cancellationToken)
    {
        var monthlyRequestsCount = await _readRepository.RecordRequestsView
                .GroupBy(x => x.DateRequested.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(g => g.Month, g => g.Count, cancellationToken);

        var result = MonthNames
            .Select((monthName, index) => new GetMonthlyRequestModel
            {
                Month = monthName,
                Count = monthlyRequestsCount.ContainsKey(index + 1) ? monthlyRequestsCount[index + 1] : 0
            })
            .ToList();

        return result;
    }
}
