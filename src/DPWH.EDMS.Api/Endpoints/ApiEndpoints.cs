namespace DPWH.EDMS.Api.Endpoints;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class RecordRequest
    {
        private const string Base = $"{ApiBase}/recordrequests";

        public const string Get = $"{Base}/{{id:guid}}";
        public const string Query = $"{Base}/query";
        public const string Create = $"{Base}/create";
        public const string Update = $"{Base}/update";
        public const string Delete = $"{Base}/delete";

        public static class Documents
        {
            public const string DeleteDocument = $"{Base}/{{id:guid}}/documents/delete";            
            public const string GetImages = $"{Base}/{{id:guid}}/images";
            public const string GetFiles = $"{Base}/{{id:guid}}/files";
            public const string SaveImage = $"{Base}/{{id:guid}}/images/save";
            public const string SaveFile = $"{Base}/{{id:guid}}/files/{{documentType}}";
            public const string UploadSupportingFile = $"{Base}/supportingfiles/{{documentType}}";
            public const string UpdateFileProperties = $"{Base}/{{assetId:guid}}/files/{{documentType}}/properties";
            public const string SaveFinancialFile = $"{Base}/{{assetId:guid}}/funding-history";
            public const string UpdateFinancialFileProperties = $"{Base}/{{assetId:guid}}/funding-history/properties";
        }
    }

    public static class EmployeeRecordEndpoints
    {
        private const string Base = $"{ApiBase}/EmployeeRecords";

        public const string Query = $"{Base}/Query";
        public const string QueryById = $"{Base}/{{id:guid}}";
        public const string Create = $"{Base}";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    public static class Assets
    {
        private const string Base = $"{ApiBase}/assets";

        public const string Create = Base;
        public const string Update = Base;
        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string Patch = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string UpdateStatus = $"{Base}/{{id:guid}}/updatestatus";
        public const string GetByImplementingOffice = $"{Base}/implementingoffices";
        public const string GetByRegionalOffice = $"{Base}/regionaloffices";
        public const string GetByBuildingId = $"{Base}/buildings/{{id}}";
        public const string Validate = $"{Base}/validate";

        public static class AssetDocuments
        {
            public const string DeleteDocument = $"{Base}/{{assetId:guid}}/documents/delete";
            //public const string DeleteDocument = $"{GetDocuments}/delete";
            public const string GetImages = $"{Base}/{{assetId:guid}}/images";
            public const string GetFiles = $"{Base}/{{assetId:guid}}/files";
            public const string SaveImage = $"{Base}/{{assetId:guid}}/images/save";
            public const string SaveFile = $"{Base}/{{assetId:guid}}/files/{{documentType}}";
            public const string UpdateFileProperties = $"{Base}/{{assetId:guid}}/files/{{documentType}}/properties";
            public const string SaveFinancialFile = $"{Base}/{{assetId:guid}}/funding-history";
            public const string UpdateFinancialFileProperties = $"{Base}/{{assetId:guid}}/funding-history/properties";
        }
    }

    public static class Licenses
    {
        private const string Base = $"{ApiBase}/licenses";

        public const string Status = $"{Base}/status";
    }

    public static class Lookups
    {
        private const string Base = $"{ApiBase}/lookups";

        public const string CommonPropertyType = $"{Base}";
        public const string Regions = $"{Base}/regions";
        public const string Province = $"{Regions}/{{regionCode}}/provinces";
        public const string CityOrMunicipalityWithoutProvince = $"{Regions}/{{regionCode}}/cities-municipalities";
        public const string CityOrMunicipality = $"{Base}/provinces/{{provinceCode}}/cities-municipalities";
        public const string Barangay = $"{Base}/cities-municipalities/{{cityOrMunicipalityCode}}/barangays";
        public const string ZipCode = $"{Base}/zipcode";

        public const string Agencies = $"{Base}/agencies";
        public const string RecordTypes = $"{Base}/recordtypes";
        public const string ValidIDs = $"{Base}/validids";
        public const string SecondaryIDs = $"{Base}/secondaryids";
        public const string RequestingOffices = $"{Base}/requestingoffices";
        public const string BuildingComponents = $"{Base}/buildingcomponents";
    }

    public static class Addresses
    {
        private const string Base = $"{ApiBase}/addresses";

        public const string CreateAddress = Base;
        public const string UpdateAddress = Base;
        public const string DeleteAddress = $"{Base}/{{id:int}}";
    }

    public static class Users
    {
        private const string Base = $"{ApiBase}/users";

        public const string Create = $"{Base}";
        public const string CreateWithRole = $"{Base}/createwithrole";
        public const string Deactivate = $"{Base}/deactivate";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetByEmployeeId = $"{Base}/{{employeeId}}";
        public const string Query = $"{Base}/query";
    }

    public static class Roles
    {
        private const string Base = $"{ApiBase}/roles";
        public const string GetRoles = Base;
        public const string Query = $"{Base}/query";
    }

    public static class Reports
    {
        private const string Base = $"{ApiBase}/reports";

        public const string AssetsPerRegion = $"{Base}/assetsperregion";
        public const string AssetsPerPropertyCondition = $"{Base}/assetsperpropertycondition";
        public const string AssetsPerBuildingStatus = $"{Base}/assetsperbuildingstatus";
        public const string UsersPerRole = $"{Base}/usersperrole";
        public const string Users = $"{Base}/users";
        public const string QueryInventory = $"{Base}/inventory";
        public const string QueryPropertyDetails = $"{Base}/inventory/propertydetails";
        public const string QueryFinancialDetails = $"{Base}/inventory/financialdetails";
        public const string QueryArea = $"{Base}/inventory/area";
        public const string QueryLocation = $"{Base}/inventory/location";
        public const string QueryFundingHistory = $"{Base}/inventory/fundingHistory";
        public const string QueryRentalRates = $"{Base}/rentalrates";
        public const string QueryPRIDSum = $"{Base}/PRIDSum";
        public const string QueryPRIInd = $"{Base}/PRIInd";
        public const string QueryPRIRSum = $"{Base}/PRIRSum";
        public const string QueryPRIBomEval = $"{Base}/PRIBomEval";
        public const string QueryPRIVal1 = $"{Base}/PRIVal1";
        public const string QueryPRIVal2 = $"{Base}/PRIVal2";

    }

    public static class Inspection
    {
        private const string Base = $"{ApiBase}/inspection";

        public const string Create = $"{Base}";
        public const string Update = $"{Base}";
        public const string UpdateStatus = $"{Base}/status";
        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAssetByBuildingId = $"{Base}/asset/{{buildingId}}";
        public const string GetBuildingComponentsByRequestNumber = $"{Base}/buildingComponents/{{requestNumber}}";
        public const string GetInspectorById = $"{Base}/inspectors/{{id}}";
        public const string UpdateProjectMonitoring = $"{Base}/projectmonitoring";

        public const string SaveImage = $"{Base}/{{inspectionRequestBuildingComponentId:guid}}/images/save";
        public const string SaveFile = $"{Base}/{{inspectionRequestId:guid}}/files/save";
        public const string UpdateBuildingComponent = $"{Base}/update/buildingComponent";
        public const string DeleteBuildingComponent = $"{Base}/delete/buildingComponent";
    }

    public static class Maintenance
    {
        private const string Base = $"{ApiBase}/maintenance";

        public const string Create = $"{Base}";
        public const string Update = $"{Base}";
        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetByBuildingId = $"{Base}/{{buildingId}}";
        public const string GetMaintenanceNumbersByBuildingId = $"{Base}/{{buildingId}}/maintenancerequestnumber";

        public class Documents
        {
            public const string SaveFile = $"{Base}/{{maintenanceRequestId:guid}}/files/save";
            public const string Delete = $"{Base}/{{documentId:guid}}/delete";
        }
    }

    public static class ProjectMonitoring
    {
        private const string Base = $"{ApiBase}/projectmonitoring";

        public const string Create = $"{Base}";
        public const string Update = $"{Base}";
        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetByBuildingId = $"{Base}/{{buildingId}}";
        public const string GetAssetApprovedInspection = $"{Base}/asset/{{buildingId}}";
        public const string GetProjectMonitoringMonthlyById = $"{Base}/{{projectMonitoringId:guid}}/recurring";
        public const string GetContractIdsByBuildingId = $"{Base}/{{buildingId}}/contractid";


        public class Documents
        {
            public const string SaveFile = $"{Base}/{{projectMonitoringId:guid}}/files/save";
            public const string SaveImage = $"{Base}/{{projectMonitoringBuildingComponentId:guid}}/images/save";
            public const string Delete = $"{Base}/{{documentId:guid}}/delete";
        }
    }

    public static class RentalRates
    {
        private const string Base = $"{ApiBase}/rentalrates";

        public const string Create = $"{Base}";
        public const string Update = $"{Base}";
        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{rentalRatePropertyId:guid}}";

        public class Property
        {
            private const string PropertyBase = $"{Base}/property";

            public const string Create = $"{PropertyBase}";
            public const string Update = $"{PropertyBase}";
            public const string Query = $"{PropertyBase}/query";
            public const string Get = $"{PropertyBase}/{{id:guid}}";

            public class Documents
            {
                public const string SaveFile = $"{PropertyBase}/{{rentalRatesPropertyId:guid}}/files/save";
            }
        }

        public class Documents
        {
            public const string SaveImage = $"{Base}/{{rentalRatesId:guid}}/images/save";
            public const string SaveFile = $"{Base}/{{rentalRatesId:guid}}/files/save";
            public const string Delete = $"{Base}/{{documentId:guid}}/delete";
        }
    }

    public static class System
    {
        private const string Base = $"{ApiBase}/system";

        public const string Create = $"{Base}/create";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string Query = $"{Base}/query";
    }

    public static class AuditLog
    {
        private const string Base = $"{ApiBase}/auditlog";

        public const string Query = $"{Base}/query";
        public const string Get = $"{Base}/{{id}}";
    }

    public static class DpwhIntegrations
    {
        private const string Base = $"{ApiBase}/dpwhintegrations";

        public const string Employee = $"{Base}/employees/{{employeeId}}";
        public const string EmployeeRaw = $"{Base}/employees/{{employeeId}}/raw";
        public const string Position = $"{Base}/positions";
        public const string GeoRegion = $"{Base}/georegions/{{type}}/{{id}}";
        public const string RequestingOffice = $"{Base}/requestingoffices";
        public const string RequestingOfficeSync = $"{Base}/requestingoffices/sync";
    }
    public static class ArcgisIntegrations
    {
        private const string Base = $"{ApiBase}/arcgisIntegration";

        public const string FeatureLayerMetadata = $"{Base}/queryFeatureLayerMetadata/{{serviceName}}/{{layerId:int}}";
        public const string AddDataToFeatureLayer = $"{Base}/addFeatures";
        public const string DeleteDataToFeatureLayer = $"{Base}/deleteFeature";
        public const string DeleteAllDataInFeatureLater = $"{Base}/deleteAllFeatures";
        public const string UpdateDataToFeatureLayer = $"{Base}/updateFeature";
        public const string QueryDataToFeatureLayer = $"{Base}/featurelayers/query";

    }

    public static class FinancialReports
    {
        private const string Base = $"{ApiBase}/financialreports";

        public const string InsurancePolicy = $"{Base}/insurancepolicy/{{policyNo}}";
        public const string InsuranceSummary = $"{Base}/insurancesummary/{{regionId}}/{{year:int}}";
    }

    public static class DataLibraries
    {
        private const string Base = $"{ApiBase}/datalibraries";

        public const string GetAll = Base;
        public const string Add = Base;
        public const string Update = Base;
        public const string Delete = $"{Base}/{{id}}";
        public const string Recover = $"{Base}/{{id}}";
    }

    public static class DataSync
    {
        private const string Base = $"{ApiBase}/datasync";

        public const string Agencies = $"{Base}/agencies";
        public const string RequestingOffices = $"{Base}/requestingoffices";
        public const string GeoLocation = $"{Base}/geolocations";
        public const string PisLocations = $"{Base}/pis-locations";
        public const string SyncEmployee = $"{Base}/employees/{{employeeId}}";
        public const string ArcgisSync = $"{Base}/arcgis";
        public const string Query = $"{Base}/query";
    }
}