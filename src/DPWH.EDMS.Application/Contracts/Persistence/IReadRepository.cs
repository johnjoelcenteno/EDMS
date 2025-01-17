﻿using DPWH.EDMS.Domain;
using DPWH.EDMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IReadRepository
{
    IQueryable<Signatory> SignatoriesView { get; }
    IQueryable<RecordType> RecordTypesView { get; }
    IQueryable<MenuItem> MenuItemsView { get; }
    IQueryable<RecordRequest> RecordRequestsView { get; }
    IQueryable<RecordRequestDocument> RecordRequestDocumentsView { get; }
    IQueryable<RequestedRecordReceipt> RequestedRecordReceiptsView { get; }
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
    IQueryable<UserProfileDocument> UserProfileDocumentsView { get; }    

}
