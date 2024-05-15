using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Infrastructure.Context;

public class AppDataContext : DbContext, IReadRepository, IWriteRepository
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }

    public DbSet<Asset> Assets { get; set; }
    public IQueryable<Asset> AssetsView => Assets.AsNoTracking();

    public DbSet<FinancialDetails> FinancialDetails { get; set; }

    public DbSet<FinancialDetailsDocuments> FinancialDetailsDocuments { get; set; }
    public IQueryable<FinancialDetailsDocuments> FinancialDetailsDocumentsView => FinancialDetailsDocuments.AsNoTracking();

    public DbSet<AssetImageDocument> AssetImages { get; set; }
    public IQueryable<AssetImageDocument> AssetImagesView => AssetImages.AsNoTracking();

    public DbSet<BuildingComponent> BuildingComponents { get; set; }
    public IQueryable<BuildingComponent> BuildingComponentsView => BuildingComponents.AsNoTracking();

    public DbSet<InspectionRequest> InspectionRequests { get; set; }
    public IQueryable<InspectionRequest> InspectionRequestsView => InspectionRequests.AsNoTracking();
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public IQueryable<MaintenanceRequest> MaintenanceRequestsView => MaintenanceRequests.AsNoTracking();

    public DbSet<AssetFileDocument> AssetFiles { get; set; }
    public IQueryable<AssetFileDocument> AssetFilesView => AssetFiles.AsNoTracking();

    public DbSet<GeoLocation> GeoLocation { get; set; }
    public IQueryable<GeoLocation> GeoLocationView => GeoLocation.AsNoTracking();

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

    public DbSet<InspectionRequestBuildingComponent> InspectionRequestBuildingComponents { get; set; }
    public IQueryable<InspectionRequestBuildingComponent> InspectionRequestBuildingComponentsView => InspectionRequestBuildingComponents.AsNoTracking();

    public DbSet<InspectionRequestBuildingComponentImage> InspectionRequestBuildingComponentImage { get; set; }
    public IQueryable<InspectionRequestBuildingComponentImage> InspectionRequestBuildingComponentImageView => InspectionRequestBuildingComponentImage.AsNoTracking();
    public DbSet<MaintenanceRequestBuildingComponent> MaintenanceRequestBuildingComponents { get; set; }
    public IQueryable<MaintenanceRequestBuildingComponent> MaintenanceRequestBuildingComponentsView => MaintenanceRequestBuildingComponents.AsNoTracking();
    public DbSet<ProjectMonitoring> ProjectMonitoring { get; set; }
    public IQueryable<ProjectMonitoring> ProjectMonitoringView => ProjectMonitoring.AsNoTracking();
    public DbSet<ProjectMonitoringScope> ProjectMonitoringScopes { get; set; }
    public IQueryable<ProjectMonitoringScope> ProjectMonitoringScopesView => ProjectMonitoringScopes.AsNoTracking();
    public DbSet<ProjectMonitoringDocument> ProjectMonitoringDocuments { get; set; }
    public IQueryable<ProjectMonitoringDocument> ProjectMonitoringDocumentView => ProjectMonitoringDocuments.AsNoTracking();
    public DbSet<InspectionRequestProjectMonitoringScopesImage> InspectionRequestProjectMonitoringScopesImage { get; set; }
    public IQueryable<InspectionRequestProjectMonitoringScopesImage> InspectionRequestProjectMonitoringScopesImageView => InspectionRequestProjectMonitoringScopesImage.AsNoTracking();
    public DbSet<DataSyncLog> DataSyncLogs { get; set; }
    public IQueryable<DataSyncLog> DataSyncLogsView => DataSyncLogs.AsNoTracking();
    public DbSet<RentalRate> RentalRates { get; set; }
    public IQueryable<RentalRate> RentalRatesView => RentalRates.AsNoTracking();
    public DbSet<RentalRateProperty> RentalRateProperty { get; set; }
    public IQueryable<RentalRateProperty> RentalRatePropertyView => RentalRateProperty.AsNoTracking();
    public DbSet<RentalRatePropertyDocument> RentalRatePropertyDocuments { get; set; }
    public IQueryable<RentalRatePropertyDocument> RentalRatePropertyDocumentsView => RentalRatePropertyDocuments.AsNoTracking();

    public DbSet<RentalRateImageDocument> RentalRatesImages { get; set; }
    public IQueryable<RentalRateImageDocument> RentalRatesImagesView => RentalRatesImages.AsNoTracking();
    public DbSet<RentalRateFileDocument> RentalRatesFiles { get; set; }
    public IQueryable<RentalRateFileDocument> RentalRatesFilesView => RentalRatesFiles.AsNoTracking();
    public DbSet<MaintenanceRequestDocument> MaintenanceRequestDocuments { get; set; }
    public IQueryable<MaintenanceRequestDocument> MaintenanceRequestDocumentsView => MaintenanceRequestDocuments.AsNoTracking();
    public DbSet<InspectionRequestDocument> InspectionRequestDocuments { get; set; }
    public IQueryable<InspectionRequestDocument> InspectionRequestDocumentsView => InspectionRequestDocuments.AsNoTracking();
    public DbSet<InspectionRequestProjectMonitoringScope> InspectionRequestProjectMonitoringScopes { get; set; }
    public IQueryable<InspectionRequestProjectMonitoringScope> InspectionRequestProjectMonitoringScopesView => InspectionRequestProjectMonitoringScopes.AsNoTracking();
    public DbSet<InspectionRequestProjectMonitoring> InspectionRequestProjectMonitoring { get; set; }
    public IQueryable<InspectionRequestProjectMonitoring> InspectionRequestProjectMonitoringView => InspectionRequestProjectMonitoring.AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetDocument>()
               .ToTable("AssetDocuments")
               .HasDiscriminator<string>("Category")
               .HasValue<AssetImageDocument>(AssetDocumentCategory.Image.ToString())
               .HasValue<AssetFileDocument>(AssetDocumentCategory.File.ToString());

        modelBuilder.Entity<RentalRateDocument>()
                .ToTable("RentalRatesDocuments")
                .HasDiscriminator<string>("Category")
                .HasValue<RentalRateImageDocument>(AssetDocumentCategory.Image.ToString())
                .HasValue<RentalRateFileDocument>(AssetDocumentCategory.File.ToString());

        base.OnModelCreating(modelBuilder);

        SetDefaultDecimalPrecision(modelBuilder);

        modelBuilder.Entity<RentalRate>()
            .Property(r => r.CapitalizationRate)
            .HasPrecision(18, 4);

        modelBuilder.Entity<RentalRate>()
            .Property(r => r.CapitalizationRatePercentage)
            .HasPrecision(18, 4);
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