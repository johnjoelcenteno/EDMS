using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.DataSource;

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
}