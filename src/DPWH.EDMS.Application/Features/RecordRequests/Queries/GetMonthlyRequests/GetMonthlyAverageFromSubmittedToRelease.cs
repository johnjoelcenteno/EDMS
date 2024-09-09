using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests
{
    public record GetMonthlyAverageFromSubmittedToReleaseRequest() : IRequest<List<MonthlyRequestAverageTimeModel>>;
    public class GetMonthlyAverageFromSubmittedToRelease : IRequestHandler<GetMonthlyAverageFromSubmittedToReleaseRequest, List<MonthlyRequestAverageTimeModel>>
    {
        private readonly IReadRepository _readRepository;

        public GetMonthlyAverageFromSubmittedToRelease(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public Task<List<MonthlyRequestAverageTimeModel>> Handle(GetMonthlyAverageFromSubmittedToReleaseRequest request, CancellationToken cancellationToken)
        {
            // Filter
            var filteredHrmdRequest = _readRepository.RecordRequestsView
                                            .Where(x => x.HRMDNoDaysUntilReleased != null);

            var filteredRmdRequest = _readRepository.RecordRequestsView
                                            .Where(x => x.RMDNoDaysUntilReleased != null);

            // Get average
            var hrmdMonthlyAverage = filteredHrmdRequest
                                        .GroupBy(x => x.Created.Month)
                                        .Select(group => new
                                        {
                                            Month = group.Key,
                                            Average = group.Average(x => x.RMDNoDaysUntilReleased)
                                        })
                                        .OrderBy(x => x.Month)
                                        .ToList();

            var rmdMonthlyAverage = filteredRmdRequest
                                        .GroupBy(x => x.Created.Month)
                                        .Select(group => new
                                        {
                                            Month = group.Key,
                                            Average = group.Average(x => x.RMDNoDaysUntilReleased)
                                        })
                                        .OrderBy(x => x.Month)
                                        .ToList();

            List<MonthlyRequestAverageTimeModel> graphData = new();
            var blankGraphData = new MonthlyRequestAverageTimeModel() { HRMDMonthlyAverage = null, RMDMonthlyAverage = null, Both = null };
            for (int month = 1; month <= 12; month++)
            {
                var hrmdMonthly = hrmdMonthlyAverage.FirstOrDefault(x => x.Month == month); 
                var rmdMonthly = rmdMonthlyAverage.FirstOrDefault(x => x.Month == month);
                if (hrmdMonthly == null || rmdMonthly == null)
                {
                    graphData.Add(blankGraphData);
                    continue;
                }

                graphData.Add(new()
                {
                    HRMDMonthlyAverage = hrmdMonthly.Average,
                    RMDMonthlyAverage = rmdMonthly.Average,
                    Both = (hrmdMonthly.Average + rmdMonthly.Average) / 2
                });
            }

            return Task.FromResult(graphData);
        }
    }
}
