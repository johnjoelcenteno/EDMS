namespace DPWH.EDMS.Infrastructure.Storage;
public static class WellKnownContainers
{
    public static readonly string WorkOrders = "workorders";
    public static readonly string AssetDocuments = "assetdocuments";
    public static readonly string FinancialDocuments = "financialdocuments";
    public static readonly string InspectionRequestDocuments = "inspectionrequestdocuments";
    public static readonly string InspectionRequestBuildingComponentImages = "inspectionrequestbuildingcomponentimages";
    public static readonly string RentalRateDocuments = "rentalratedocuments";
    public static readonly string RentalRatePropertyDocuments = "rentalratepropertydocuments";
    public static readonly string MaintenanceRequestDocuments = "maintenancerequestdocuments";
    public static readonly string ProjectMonitoringDocuments = "projectmonitoringdocuments";
    public static readonly string ProjectMonitoringBuildingComponentImages = "projectmonitoringbuildingcomponentimages";

}

public static class WellKnownQueues
{
    public static readonly string EmailNotifications = "notifications";
}
public static class WellKnownTables
{
    public static readonly string WorkOrderRequests = "workorderrequests";
}