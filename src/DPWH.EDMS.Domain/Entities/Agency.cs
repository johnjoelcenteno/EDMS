using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class Agency : EntityBase
{
    private Agency() { }

    private Agency(string agencyId, string agencyName, string attachedAgencyId, string attachedAgencyName)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        AgencyName = agencyName;
        AttachedAgencyId = attachedAgencyId;
        AttachedAgencyName = attachedAgencyName;
    }

    public static Agency Create(string agencyId, string agencyName, string attachedAgencyId, string attachedAgencyName, string createdBy)
    {
        var agency = new Agency(agencyId, agencyName, attachedAgencyId, attachedAgencyName);
        agency.SetCreated(createdBy);

        return agency;
    }

    public void Update(string agencyId, string agencyName, string attachedAgencyId, string attachedAgencyName, string modifiedBy)
    {
        AgencyId = agencyId;
        AgencyName = agencyName;
        AttachedAgencyId = attachedAgencyId;
        AttachedAgencyName = attachedAgencyName;

        SetModified(modifiedBy);
    }

    public void SetCodes(string numberCode, string departmentCode, string modifiedBy)
    {
        AgencyNumberCode = numberCode;
        AgencyCode = departmentCode;

        SetModified(modifiedBy);
    }

    public string? AgencyNumberCode { get; set; }
    public string? AgencyCode { get; set; }
    public string AgencyId { get; set; }
    public string AgencyName { get; set; }
    public string AttachedAgencyId { get; set; }
    public string AttachedAgencyName { get; set; }
}