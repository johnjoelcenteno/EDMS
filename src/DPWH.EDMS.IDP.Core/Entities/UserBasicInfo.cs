using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.IDP.Core.Entities;

public class UserBasicInfo
{
    private UserBasicInfo(string? firstName, string? middleInitial, string? lastName)
    {
        FirstName = firstName;
        MiddleInitial = middleInitial;
        LastName = lastName;
    }

    public static UserBasicInfo Create(string? firstName, string? middleInitial, string? lastName)
    {
        return new UserBasicInfo(firstName, middleInitial, lastName);
    }

    public string? Id { get; private set; }
    [StringLength(50)]
    public string? FirstName { get; private set; }
    [StringLength(10)]
    public string? MiddleInitial { get; private set; }
    [StringLength(50)]
    public string? LastName { get; private set; }
}