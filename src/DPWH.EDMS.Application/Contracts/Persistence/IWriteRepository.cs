using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IWriteRepository
{
    DbSet<Signatory> Signatories { get; }
    DbSet<EmployeeRecord> EmployeeRecords { get; }
    DbSet<RecordRequest> RecordRequests { get; }
    DbSet<RecordRequestDocument> RecordRequestDocuments { get; }
    DbSet<RequestedRecord> RequestedRecords { get; }
    DbSet<RequestedRecordReceipt> RequestedRecordReceipts { get; }
    DbSet<RecordType> RecordTypes { get; }
    DbSet<Record> Records { get; }    
    DbSet<GeoLocation> Geolocations { get; }
    DbSet<ConfigSetting> ConfigSettings { get; }
    DbSet<SystemLog> SystemLogs { get; }
    DbSet<DataLibrary> DataLibraries { get; }
    DbSet<RequestingOffice> RequestingOffices { get; }
    DbSet<ChangeLog> ChangeLogs { get; }
    DbSet<Agency> Agencies { get; }    
    DbSet<DataSyncLog> DataSyncLogs { get; }
    DbSet<MenuItem> MenuItems { get; }
    DbSet<UserProfileDocument> UserProfileDocuments { get; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
