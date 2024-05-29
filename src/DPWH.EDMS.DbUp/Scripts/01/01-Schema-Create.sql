IF NOT EXISTS (
	SELECT
		*
	FROM
		sysobjects
	WHERE
		name = 'EmployeeRecords'
		AND xtype = 'U'
) BEGIN CREATE TABLE EmployeeRecords (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50),
	LastName NVARCHAR(50) NOT NULL,
	Office NVARCHAR(100),
	Email NVARCHAR(100),
	MobileNumber NVARCHAR(15),
	EmployeeNumber NVARCHAR(50) NOT NULL UNIQUE,
	EmployeeId NVARCHAR(75) NOT NULL UNIQUE,
	RegionCentralOffice NVARCHAR(100),
	DistrictBureauService NVARCHAR(100),
	Position NVARCHAR(100),
	Designation NVARCHAR(100),
	Role NVARCHAR(150),
	UserAccess NVARCHAR(50),
	Department NVARCHAR(150),
	RegionalOfficeRegion NVARCHAR(150),
	RegionalOfficeProvince NVARCHAR(150),
	DistrictEngineeringOffice NVARCHAR(150),
	DesignationTitle NVARCHAR(150),
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(50),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(50) NULL
);

END IF NOT EXISTS (
	SELECT
		*
	FROM
		sysobjects
	WHERE
		name = 'DocumentRecords'
		AND xtype = 'U'
) BEGIN CREATE TABLE DocumentRecords (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	Title NVARCHAR(100) NOT NULL,
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(50),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(50)
);

END IF NOT EXISTS (
	SELECT
		*
	FROM
		sysobjects
	WHERE
		name = 'DocumentRequests'
		AND xtype = 'U'
) BEGIN CREATE TABLE DocumentRequests (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ControlNumber NVARCHAR(50) NOT NULL,
	EmployeeNumber NVARCHAR(50) NOT NULL,
	EmployeeRecordsId UNIQUEIDENTIFIER NOT NULL,
	ClaimedBy NVARCHAR(100),
	AuthorizedRepresentative NVARCHAR(100),
	ValidId NVARCHAR(100),
	SupportingDocument NVARCHAR(100),
	DocumentRecordsId UNIQUEIDENTIFIER NOT NULL,
	DateRequested DATETIME NOT NULL,
	RequestedRecord NVARCHAR(100),
	Purpose NVARCHAR(100),
	Status NVARCHAR(50),
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(50),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(50) NULL,
);

END