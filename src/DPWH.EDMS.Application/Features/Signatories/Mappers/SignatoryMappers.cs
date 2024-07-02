using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application;

public static class SignatoryMappers
{
    public static QuerySignatoryModel Map(Signatory signatory)
    {
        return new QuerySignatoryModel
        {
            Id = signatory.Id,
            DocumentType = signatory.DocumentType,
            Name = signatory.Name,
            Position = signatory.Position,
            Office1 = signatory.Office1,
            Office2 = signatory.Office2,
            SignatoryNo = signatory.SignatoryNo,
            IsActive = signatory.IsActive,
        };
    }
}
