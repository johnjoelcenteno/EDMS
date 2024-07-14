using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Infrastructure.Context;

public class AppDataContext : DbContext, IReadRepository, IWriteRepository
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }
    public DbSet<RecordRequest> RecordRequests { get; set; }
    public IQueryable<RecordRequest> RecordRequestsView => RecordRequests.Include(r => r.AuthorizedRepresentative).AsNoTracking();
    public DbSet<RecordRequestDocument> RecordRequestDocuments { get; set; }
    public IQueryable<RecordRequestDocument> RecordRequestDocumentsView => RecordRequestDocuments.AsNoTracking();
    public DbSet<RequestedRecord> RequestedRecords { get; set; }
    public IQueryable<RequestedRecord> RequestedRecordsView => RequestedRecords.AsNoTracking();
    public DbSet<Record> Records { get; set; }
    public IQueryable<Record> RecordsView => Records.AsNoTracking();

    public DbSet<EmployeeRecord> EmployeeRecords { get; set; }
    public IQueryable<EmployeeRecord> EmployeeRecordsView => EmployeeRecords.AsNoTracking();

    public DbSet<GeoLocation> Geolocations { get; set; }
    public IQueryable<GeoLocation> GeolocationsView => Geolocations.AsNoTracking();

    public DbSet<ConfigSetting> ConfigSettings { get; set; }
    public IQueryable<ConfigSetting> ConfigSettingsView => ConfigSettings.AsNoTracking();

    public DbSet<SystemLog> SystemLogs { get; set; }
    public IQueryable<SystemLog> SystemLogsView => SystemLogs.AsNoTracking();

    public DbSet<DataLibrary> DataLibraries { get; set; }
    public IQueryable<DataLibrary> DataLibrariesView => DataLibraries.AsNoTracking();

    public DbSet<RequestingOffice> RequestingOffices { get; set; }
    public IQueryable<RequestingOffice> RequestingOfficesView => RequestingOffices.AsNoTracking();

    public DbSet<ChangeLog> ChangeLogs { get; set; }
    public IQueryable<ChangeLog> ChangeLogsView => ChangeLogs.AsNoTracking();

    public DbSet<ChangeLogItem> ChangeLogItems { get; set; }
    public IQueryable<ChangeLogItem> ChangeLogItemsView => ChangeLogItems.AsNoTracking();

    public DbSet<Agency> Agencies { get; set; }
    public IQueryable<Agency> AgenciesView => Agencies.AsNoTracking();
    
    public DbSet<DataSyncLog> DataSyncLogs { get; set; }
    public IQueryable<DataSyncLog> DataSyncLogsView => DataSyncLogs.AsNoTracking();    
    public DbSet<RecordType> RecordTypes { get; set; }
    public IQueryable<RecordType> RecordTypesView => RecordTypes.AsNoTracking();

    public DbSet<Signatory> Signatories { get; set; }
    public IQueryable<Signatory> SignatoriesView => Signatories.AsNoTracking();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorizedRepresentative>()
            .ToTable("RecordRequests");

        modelBuilder.Entity<RecordRequest>(b =>
        {
            b.HasOne(u => u.AuthorizedRepresentative)
                .WithOne()
                .HasForeignKey<AuthorizedRepresentative>(a => a.Id);
            b.Navigation(u => u.AuthorizedRepresentative).IsRequired();
        });

        base.OnModelCreating(modelBuilder);

        SetDefaultDecimalPrecision(modelBuilder);
    }

    private void SetDefaultDecimalPrecision(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }
}