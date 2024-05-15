using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.IDP.Core.Extensions;

namespace DPWH.EDMS.Application.Features.RequestingOffices.Commands.BatchCreateRequestingOffice;

public record BatchCreateRequestingOfficeCommand(RequestingOfficeResponse? RequestingOffices, bool EnableCleanUp) : IRequest;

internal sealed class BatchCreateRequestingOfficeHandler : IRequestHandler<BatchCreateRequestingOfficeCommand>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private const string RequestingOffices = "Requesting Offices";

    public BatchCreateRequestingOfficeHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task Handle(BatchCreateRequestingOfficeCommand request, CancellationToken cancellationToken)
    {
        var syncResult = "Success";
        var syncDescription = "";
        int syncedDataCount = 0;
        try
        {
            if (request.EnableCleanUp)
            {
                await _repository.RequestingOffices.ExecuteDeleteAsync(cancellationToken);
            }

            var requestingOffices = request.RequestingOffices!.Body!.Response!.Result!.Data!
                .Select(d => RequestingOffice.Create(
                    d.SubOfficeId!,
                    d.SubOfficeName!,
                    d.OfficeId,
                    RegionalOffice.List.FirstOrDefault(r => r.Name == d.SubOfficeName)?.NumberCode,
                    _principal.GetUserName()))
                .ToList();

            EnsureCentralOfficeIncluded(requestingOffices);

            var entitiesToAdd = !request.EnableCleanUp
                ? await UpdateIfExists(requestingOffices, cancellationToken)
                : requestingOffices;

            var offices = entitiesToAdd.OrderBy(e => e.ParentId);
            foreach (var office in offices)
            {
                syncedDataCount++;
            }
            await _repository.RequestingOffices.AddRangeAsync(offices, cancellationToken);
            syncDescription = syncedDataCount == 0
           ? "No data updated. Already up to date."
           : $"Successfully synced {syncedDataCount} row{(syncedDataCount > 1 ? "s" : "")} of data.";
        }
        catch (AppException e)
        {
            syncResult = "Error";
            syncDescription = e.Message;
        }
        finally
        {
            var syncLog = DataSyncLog.Create(RequestingOffices, syncResult, syncDescription, _principal.GetUserName());
            await _repository.DataSyncLogs.AddAsync(syncLog, cancellationToken);
        }
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<RequestingOffice>> UpdateIfExists(IEnumerable<RequestingOffice> requestingOffices, CancellationToken cancellationToken)
    {
        var toAdd = new List<RequestingOffice>();

        foreach (var ro in requestingOffices)
        {
            var entity = await _repository.RequestingOffices
                .FirstOrDefaultAsync(r => r.Id == ro.Id, cancellationToken);

            if (entity is null)
            {
                toAdd.Add(ro);
                continue;
            }

            entity.Update(ro, _principal.GetUserName());
            _repository.RequestingOffices.Update(entity);
        }

        return toAdd;
    }

    private void EnsureCentralOfficeIncluded(ICollection<RequestingOffice> requestingOffices)
    {
        var (centralOfficeId, centralOfficeName) = ("OI10", "Central Office");

        if (requestingOffices.All(o => o.Id != centralOfficeId))
        {
            requestingOffices.Add(RequestingOffice.Create(
                centralOfficeId,
                centralOfficeName,
                null,
                null,
                _principal.GetUserName()));
        }
    }
}