using DPWH.EDMS.Domain.Common;
using UUIDNext;

namespace DPWH.EDMS.Domain;

public class Signatory : EntityBase
{
    private Signatory() { }

    public static Signatory Create(
        string documentType,
        string name,
        string position,
        string office1,
        string createdBy,
        string? office2 = null,
        int signatoryNo = 0
    )
    {
        var entity = new Signatory
        {
            Id = Uuid.NewDatabaseFriendly(Database.SqlServer),
            DocumentType = documentType,
            Name = name,
            Position = position,
            Office1 = office1,
            Office2 = office2,
            SignatoryNo = signatoryNo,
            IsActive = true
        };
        entity.SetCreated(createdBy);
        return entity;
    }

    public string DocumentType { get; private set; }
    public string Name { get; private set; }
    public string Position { get; private set; }
    public string Office1 { get; private set; }
    public string? Office2 { get; private set; }
    public int SignatoryNo { get; private set; }
    public bool IsActive { get; private set; }
    public string? UriSignature { get; set; }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetModified(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        IsActive = true;
        SetModified(updatedBy);
    }

    public void UpdateDetails(
        string documentType,
        string name,
        string position,
        string office1,
        string? office2,
        int signatoryNo,
        string updatedBy
    )
    {
        DocumentType = documentType;
        Name = name;
        Position = position;
        Office1 = office1;
        Office2 = office2;
        SignatoryNo = signatoryNo;
        SetModified(updatedBy);
    }
    
    public void UpdateUriSignature(string uriSignature)
    {
        UriSignature = uriSignature;
    }
}