using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace DPWH.EDMS.Client.Shared.MockModels;
public class RecordModel
{
    public string Id { get; set; }
    public string Role { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Office { get; set; }
    public string EmailAddress { get; set; }
    public string ContactNumber { get; set; }
    public string BureauServiceDivisionSectionUnit { get; set; }
    public List<Document> Documents { get; set; }
}

public class Document
{
    public int Id { get; set; }
    public string DocumentName { get; set; }
    public string ControlNumber { get; set; }
    public DateTime DateRequested { get; set; }
}

public class PersonalRecordDocument
{
    public string Id { get; set; }
    public string DocumentName { get; set; }
}
public class ModelData
{
    public double Series1 { get; set; }
    public double Series2 { get; set; }
    public double Series3 { get; set; }
}
 
 

public class MockData
{
    public static RecordModel[] GetRecords()
    {
        return new RecordModel[]
        {
            new RecordModel
            {
                Id = "R1",
                Role = "End User",
                LastName = "Rosales",
                FirstName = "Karen",
                MiddleName = "Mesinas",
                EmailAddress = "karen.rosales@gmail.com",
                ContactNumber = "+63 900 000 0000",
                Office = "Headquarters",
                BureauServiceDivisionSectionUnit = "Administration Division",
                Documents = GenerateDocuments()
            },
            new RecordModel
            {
                Id = "R2",
                Role = "End User",
                LastName = "Jeresano",
                FirstName = "Edmar",
                MiddleName = "Alano",
                EmailAddress = "edmar.jeresano@gmail.com",
                ContactNumber = "+63 900 000 0000",
                Office = "Regional Office",
                BureauServiceDivisionSectionUnit = "Human Resources Bureau",
                Documents = GenerateDocuments()
            },
            new RecordModel
            {
                Id = "R3",
                Role = "End User",
                LastName = "Aluan",
                FirstName = "Nataniel",
                MiddleName = "Girado",
                EmailAddress = "nataniel.aluan@gmail.com",
                ContactNumber = "+63 900 000 0000",
                Office = "Branch Office",
                BureauServiceDivisionSectionUnit = "Finance Service",
                Documents = GenerateDocuments()
            },
            new RecordModel
            {
                Id = "R4",
                Role = "End User",
                LastName = "Millano",
                FirstName = "Darwin",
                MiddleName = "Evangelista",
                EmailAddress = "darwin.millano@gmail.com",
                ContactNumber = "+63 900 000 0000",
                Office = "District Office",
                BureauServiceDivisionSectionUnit = "Operations Division",
                Documents = GenerateDocuments()
            },
            new RecordModel
            {
                Id = "R5",
                Role = "End User",
                LastName = "Belmonte",
                FirstName = "Kevin",
                MiddleName = "Santos",
                EmailAddress = "kevin.belmonte@gmail.com",
                ContactNumber = "+63 900 000 0000",
                Office = "Satellite Office",
                BureauServiceDivisionSectionUnit = "Technical Support Unit",
                Documents = GenerateDocuments()
            }
        };
    }

    public static List<Document> GenerateDocuments()
    {
        return new List<Document>
        {
            new Document { Id = 1, DocumentName = "Approved Appointments", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-10) },
            new Document { Id = 2, DocumentName = "Position Description Form (PDF)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-9) },
            new Document { Id = 3, DocumentName = "Service Records", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-8) },
            new Document { Id = 4, DocumentName = "Employee Leave Card", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-7) },
            new Document { Id = 5, DocumentName = "Leave Application (Terminal Leave only)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-6) },
            new Document { Id = 6, DocumentName = "Notice of Salary Adjustment (NOSA)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-5) },
            new Document { Id = 7, DocumentName = "Personal Data Sheet (PDS) / Information Sheet", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-4) },
            new Document { Id = 8, DocumentName = "Administrative Case / Civil Case/Decisions", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-3) },
            new Document { Id = 9, DocumentName = "Training Certificates / Ratings", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-2) },
            new Document { Id = 10, DocumentName = "Travel Directive, Certificate of Appearance", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-1) },
            new Document { Id = 11, DocumentName = "Authority to Travel for Personal Reason", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-20) },
            new Document { Id = 12, DocumentName = "Memorandum Receipt for Equipment (MR)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-19) },
            new Document { Id = 13, DocumentName = "GSIS Forms (Retirement, Information for Membership)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-18) },
            new Document { Id = 14, DocumentName = "Memo (Designation, Directive)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-17) },
            new Document { Id = 15, DocumentName = "Daily Wage Appointment/Plantilla", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-16) },
        };
    }

    public static List<ModelData> MonthlyRequestAverageTimeList()
    {
        return new List<ModelData>
        {
            new ModelData()
            {
                Series1 = 2,
                Series2 = 4,
                Series3 = 3

            },
            new ModelData()
            {
                 Series1 = 2,
                Series2 = 7,
                Series3 = 5
            },
            new ModelData()
            {
                Series1 = 5,
                Series2 = 11,
                Series3 = 6
            },
            new ModelData()
            {
                Series1 = 7,
                Series2 = 19,
                Series3 = 12
            },
            new ModelData()
            {
                Series1 = 5,
                Series2 = 11,
                Series3 = 6
            },
            new ModelData()
            {
                Series1 = 14,
                Series2 = 40,
                Series3 = 26
            },
            new ModelData()
            {
                Series1 = 15,
                Series2 = 27,
                Series3 = 12
            },
            
        };
    }
    
}

public class MockCurrentData
{
    public static RecordModel GetCurrentRecord()
    {
        return new RecordModel
        {
            Id = "R1",
            Role = "Super Admin",
            LastName = "Doe",
            FirstName = "Jane",
            MiddleName = "Smith",
            Office = "Headquarters",
            EmailAddress = "jane.doe@gmail.com",
            ContactNumber = "+63 900 000 0000",
            BureauServiceDivisionSectionUnit = "Administrative Division"
        };
    }

    public static List<Document> GetDocuments()
    {
        return new List<Document>
        {
            new Document { Id = 1, DocumentName = "Approved Appointments", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-10) },
            new Document { Id = 2, DocumentName = "Position Description Form (PDF)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-9) },
            new Document { Id = 3, DocumentName = "Service Records", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-8) },
            new Document { Id = 4, DocumentName = "Employee Leave Card", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-7) },
            new Document { Id = 5, DocumentName = "Leave Application (Terminal Leave only)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-6) },
            new Document { Id = 6, DocumentName = "Notice of Salary Adjustment (NOSA)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-5) },
            new Document { Id = 7, DocumentName = "Personal Data Sheet (PDS) / Information Sheet", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-4) },
            new Document { Id = 8, DocumentName = "Administrative Case / Civil Case/Decisions", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-3) },
            new Document { Id = 9, DocumentName = "Training Certificates / Ratings", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-2) },
            new Document { Id = 10, DocumentName = "Travel Directive, Certificate of Appearance", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-1) },
            new Document { Id = 11, DocumentName = "Authority to Travel for Personal Reason", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-20) },
            new Document { Id = 12, DocumentName = "Memorandum Receipt for Equipment (MR)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-19) },
            new Document { Id = 13, DocumentName = "GSIS Forms (Retirement, Information for Membership)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-18) },
            new Document { Id = 14, DocumentName = "Memo (Designation, Directive)", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-17) },
            new Document { Id = 15, DocumentName = "Daily Wage Appointment/Plantilla", ControlNumber = "CN2024000000", DateRequested = DateTime.Now.AddDays(-16) },
        };
    }

    public static List<PersonalRecordDocument> GenerateCurrentDocuments()
    {
        return new List<PersonalRecordDocument>
    {
        new PersonalRecordDocument { Id = "1", DocumentName = "Administrative Case / Civil case Decisions" },
        new PersonalRecordDocument { Id = "2", DocumentName = "Appointment/Oath of Office" },
        new PersonalRecordDocument { Id = "3", DocumentName = "Approved Appointments" },
        new PersonalRecordDocument { Id = "4", DocumentName = "Authority to Travel for Personal Reason" },
        new PersonalRecordDocument { Id = "5", DocumentName = "Certificate of Employer-Employee Relationship" },
        new PersonalRecordDocument { Id = "6", DocumentName = "Certificate of Employment" },
        new PersonalRecordDocument { Id = "7", DocumentName = "Certificate of Employment and Compensation" },
        new PersonalRecordDocument { Id = "8", DocumentName = "Certificate of Last Day in Service" },
        new PersonalRecordDocument { Id = "9", DocumentName = "Certificate of Leave Balance" },
        new PersonalRecordDocument { Id = "10", DocumentName = "Certificate of Leave Without Pay" },
        new PersonalRecordDocument { Id = "11", DocumentName = "Certificate of Transfer of Leave Credits" },
        new PersonalRecordDocument { Id = "12", DocumentName = "Certification of No Loan Deductions" },
        new PersonalRecordDocument { Id = "13", DocumentName = "Certification Under Oath for unavailable documents" },
        new PersonalRecordDocument { Id = "14", DocumentName = "Contract of Service" },
        new PersonalRecordDocument { Id = "15", DocumentName = "Daily Wage Appointment / Plantilla" },
        new PersonalRecordDocument { Id = "16", DocumentName = "Employee Leave Card" },
        new PersonalRecordDocument { Id = "17", DocumentName = "GSIS Forms (Retirement Information for Membership)" },
        new PersonalRecordDocument { Id = "18", DocumentName = "Leave Application (Terminal Leave only)" },
        new PersonalRecordDocument { Id = "19", DocumentName = "Memo (Designation, Directive)" },
        new PersonalRecordDocument { Id = "20", DocumentName = "Memorandum Receipt for Equipment (MT)" },
        new PersonalRecordDocument { Id = "21", DocumentName = "Notice of Salary Adjustment (NOSA)" },
        new PersonalRecordDocument { Id = "22", DocumentName = "Personal Data Sheet (PDS) / Information Sheet (Must be latest)" },
        new PersonalRecordDocument { Id = "23", DocumentName = "Position Description Form (PDF)" },
        new PersonalRecordDocument { Id = "24", DocumentName = "RA 5830 Certification" },
        new PersonalRecordDocument { Id = "25", DocumentName = "Salary Index" },
        new PersonalRecordDocument { Id = "26", DocumentName = "Service Record (Active)" },
        new PersonalRecordDocument { Id = "27", DocumentName = "Service Record (Retired)" },
        new PersonalRecordDocument { Id = "28", DocumentName = "Training Certificates / Ratings" },
        new PersonalRecordDocument { Id = "29", DocumentName = "Travel Directive, Certificate of Appearance" },
    };
    }
}