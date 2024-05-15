using System.Globalization;
using FluentValidation;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogs;

public class GetAuditLogsQueryValidator : AbstractValidator<GetAuditLogsQuery>
{
    public GetAuditLogsQueryValidator()
    {
        var dateFormat = GetAuditLogsHandler.ExpectedDateFormat;

        RuleFor(query => query.DataSourceRequest)
            .NotNull()
            .WithMessage("Query must not be null or empty.")
            .ChildRules(dsr =>
            {
                dsr.RuleFor(d => d.Filter)
                    .NotNull()
                    .WithMessage("Filter must not be empty or null.")
                    .ChildRules(f =>
                    {
                        f.RuleFor(i => i.Filters)
                            .NotNull()
                            .WithMessage("Date filters must not be empty or null.")
                            .Must(dates => dates != null && dates.Count() == 2)
                            .WithMessage("Date filters must contain exactly 2 values.")
                            .Must(dates => dates.All(date => DateTimeOffset.TryParse(date.Value.ToString(), CultureInfo.InvariantCulture, out _)))
                            .WithMessage("All dates in the collection must be parseable to valid dates.")
                            .Must(dates =>
                            {
                                if (dates.Count() == 2
                                    && DateTimeOffset.TryParse(dates.ElementAt(0).Value.ToString(), CultureInfo.InvariantCulture, out var firstDate)
                                    && DateTimeOffset.TryParse(dates.ElementAt(1).Value.ToString(), CultureInfo.InvariantCulture, out var secondDate))
                                {
                                    if (firstDate.Date == secondDate.Date)
                                    {
                                        secondDate = secondDate.Date.AddDays(1).AddSeconds(-1);
                                    }
                                    return firstDate < secondDate;
                                }
                                return false;
                            }).WithMessage("The first date should be earlier than the second date.");
                        // .Must(value => GetAuditLogsHandler.Categories.Contains(value.ToString()))
                        // .WithMessage($"Category/Entity must be one of {string.Join(", ", GetAuditLogsHandler.Categories)}.");
                    });
            });
    }
}