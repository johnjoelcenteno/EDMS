using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IWriteRepository
{
    DbSet<EmployeeRecord> EmployeeRecords { get; }
    DbSet<RecordRequest> RecordRequests { get; }
    DbSet<RecordRequestDocument> RecordRequestDocuments { get; }
    DbSet<RequestedRecord> RequestedRecords { get; }
    DbSet<RecordType> RecordTypes { get; }
    DbSet<Record> Records { get; }

    DbSet<Asset> Assets { get; }
    DbSet<InspectionRequest> InspectionRequests { get; }
    DbSet<InspectionRequestDocument> InspectionRequestDocuments { get; }
    DbSet<MaintenanceRequest> MaintenanceRequests { get; }
    DbSet<AssetImageDocument> AssetImages { get; }
    DbSet<AssetFileDocument> AssetFiles { get; }
    DbSet<FinancialDetails> FinancialDetails { get; }
    DbSet<FinancialDetailsDocuments> FinancialDetailsDocuments { get; }
    DbSet<GeoLocation> Geolocations { get; }
    DbSet<ConfigSetting> ConfigSettings { get; }
    DbSet<SystemLog> SystemLogs { get; }
    DbSet<DataLibrary> DataLibraries { get; }
    DbSet<RequestingOffice> RequestingOffices { get; }
    DbSet<ChangeLog> ChangeLogs { get; }
    DbSet<Agency> Agencies { get; }
    DbSet<BuildingComponent> BuildingComponents { get; }
    DbSet<InspectionRequestBuildingComponent> InspectionRequestBuildingComponents { get; }
    DbSet<InspectionRequestBuildingComponentImage> InspectionRequestBuildingComponentImage { get; }
    DbSet<MaintenanceRequestBuildingComponent> MaintenanceRequestBuildingComponents { get; }
    DbSet<RentalRateProperty> RentalRateProperty { get; }
    DbSet<RentalRatePropertyDocument> RentalRatePropertyDocuments { get; }
    DbSet<RentalRate> RentalRates { get; }
    DbSet<RentalRateImageDocument> RentalRatesImages { get; }
    DbSet<RentalRateFileDocument> RentalRatesFiles { get; }
    DbSet<MaintenanceRequestDocument> MaintenanceRequestDocuments { get; }
    DbSet<DataSyncLog> DataSyncLogs { get; }
    DbSet<ProjectMonitoring> ProjectMonitoring { get; }
    DbSet<ProjectMonitoringScope> ProjectMonitoringScopes { get; }
    DbSet<ProjectMonitoringDocument> ProjectMonitoringDocuments { get; }
    DbSet<InspectionRequestProjectMonitoringScopesImage> InspectionRequestProjectMonitoringScopesImage { get; }
    DbSet<InspectionRequestProjectMonitoringScope> InspectionRequestProjectMonitoringScopes { get; }
    DbSet<InspectionRequestProjectMonitoring> InspectionRequestProjectMonitoring { get; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
