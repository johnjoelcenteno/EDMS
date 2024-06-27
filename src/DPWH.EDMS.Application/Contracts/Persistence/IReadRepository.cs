﻿using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IReadRepository
{
    IQueryable<RecordType> RecordTypesView { get; }
    IQueryable<RecordRequest> RecordRequestsView { get; }
    IQueryable<RecordRequestDocument> RecordRequestDocumentsView { get; }
    IQueryable<EmployeeRecord> EmployeeRecordsView { get; }
    IQueryable<Record> RecordsView { get; }
    IQueryable<Asset> AssetsView { get; }
    IQueryable<InspectionRequest> InspectionRequestsView { get; }
    IQueryable<MaintenanceRequest> MaintenanceRequestsView { get; }
    IQueryable<BuildingComponent> BuildingComponentsView { get; }
    IQueryable<FinancialDetailsDocuments> FinancialDetailsDocumentsView { get; }
    IQueryable<GeoLocation> GeolocationsView { get; }
    IQueryable<AssetFileDocument> AssetFilesView { get; }
    IQueryable<AssetImageDocument> AssetImagesView { get; }
    IQueryable<ConfigSetting> ConfigSettingsView { get; }
    IQueryable<SystemLog> SystemLogsView { get; }
    IQueryable<DataLibrary> DataLibrariesView { get; }
    IQueryable<RequestingOffice> RequestingOfficesView { get; }
    IQueryable<ChangeLog> ChangeLogsView { get; }
    IQueryable<Agency> AgenciesView { get; }
    IQueryable<InspectionRequestBuildingComponent> InspectionRequestBuildingComponentsView { get; }
    IQueryable<DataSyncLog> DataSyncLogsView { get; }
    IQueryable<RentalRate> RentalRatesView { get; }
    IQueryable<RentalRateProperty> RentalRatePropertyView { get; }
    IQueryable<ProjectMonitoring> ProjectMonitoringView { get; }

}
