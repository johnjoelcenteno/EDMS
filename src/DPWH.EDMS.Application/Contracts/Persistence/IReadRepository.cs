using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IReadRepository
{
    IQueryable<Signatory> SignatoriesView { get; }
    IQueryable<RecordType> RecordTypesView { get; }
    IQueryable<RecordRequest> RecordRequestsView { get; }
    IQueryable<RecordRequestDocument> RecordRequestDocumentsView { get; }
    IQueryable<EmployeeRecord> EmployeeRecordsView { get; }
    IQueryable<Record> RecordsView { get; }    
    IQueryable<GeoLocation> GeolocationsView { get; }    
    IQueryable<ConfigSetting> ConfigSettingsView { get; }
    IQueryable<SystemLog> SystemLogsView { get; }
    IQueryable<DataLibrary> DataLibrariesView { get; }
    IQueryable<RequestingOffice> RequestingOfficesView { get; }
    IQueryable<ChangeLog> ChangeLogsView { get; }
    IQueryable<Agency> AgenciesView { get; }    
    IQueryable<DataSyncLog> DataSyncLogsView { get; }    

}
