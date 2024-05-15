using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Domain.Exceptions;

namespace DPWH.EDMS.Application.Features.Agencies.Commands.BatchCreateAgencies;

public record BatchCreateAgenciesCommand(NationalGovtAgencyResponse? NationalGovtAgencies, bool? EnableCleanUp = true) : IRequest;

internal sealed class BatchCreateAgenciesHandler : IRequestHandler<BatchCreateAgenciesCommand>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private const string Agencies = "Agencies";

    public BatchCreateAgenciesHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task Handle(BatchCreateAgenciesCommand request, CancellationToken cancellationToken)
    {
        var syncResults = "Success";
        var syncDescription = "";
        var syncedDataCount = 0;

        try
        {
            if (request.EnableCleanUp is true)
            {
                await _repository.Agencies.ExecuteDeleteAsync(cancellationToken);
            }

            var agencies = request.NationalGovtAgencies!.Body!.Response!.Result!.Data!
                .Select(d => Agency.Create(d.AgencyId!, d.AgencyName!, d.AttachedAgencyId!, d.AttachedAgencyName!, _principal.GetUserName()))
                .ToList();

            var departments = Departments.List();
            var departmentNames = departments.Select(d => d.Name);

            var filterAgency = agencies
                .Where(a => departmentNames.Contains(a.AgencyName)).ToList();

            foreach (var agency in filterAgency)
            {
                var department = departments.FirstOrDefault(d => d.Name == agency.AgencyName);
                agency.SetCodes(department.NumberCode, department.DepartmentCode, _principal.GetUserName());
            }

            var entitiesToAdd = request.EnableCleanUp is true
                ? agencies
                : await UpdateIfExists(departments, cancellationToken);

            foreach (var agency in entitiesToAdd)
            {
                syncedDataCount++;
            }
            syncDescription = syncedDataCount == 0
                ? "No data updated. Already up to date."
                : $"Successfully synced {syncedDataCount} row{(syncedDataCount > 1 ? "s" : "")} of data.";

            await _repository.Agencies.AddRangeAsync(entitiesToAdd, cancellationToken);
        }
        catch (AppException e)
        {
            syncResults = "Error";
            syncDescription = e.Message;
        }
        finally
        {
            var dataSync = DataSyncLog.Create(Agencies, syncResults, syncDescription, _principal.GetUserName());
            await _repository.DataSyncLogs.AddAsync(dataSync, cancellationToken);
        }
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<Agency>> UpdateIfExists(IReadOnlyList<Department> departments, CancellationToken cancellationToken)
    {
        var toAdd = new List<Agency>();
        var departmentNames = departments.Select(d => d.Name);

        var filterAgency = await _repository.Agencies
            .Where(a => departmentNames.Contains(a.AgencyName))
            .ToListAsync(cancellationToken);
        foreach (var agency in filterAgency)
        {
            var entity = await _repository.Agencies
                .FirstOrDefaultAsync(a => a.Id == agency.Id, cancellationToken);
            var department = departments.FirstOrDefault(d => d.Name == agency.AgencyName);
            agency.SetCodes(department.NumberCode, department.DepartmentCode, _principal.GetUserName());

            if (entity is null)
            {
                toAdd.Add(agency);
                continue;
            }

            entity.Update(agency.AgencyId, agency.AgencyName, agency.AttachedAgencyId, agency.AttachedAgencyName, _principal.GetUserName());
            if (department != null)
                entity.SetCodes(department.NumberCode, department.DepartmentCode, _principal.GetUserName());
            _repository.Agencies.Update(entity);
        }
        return toAdd;
    }
}