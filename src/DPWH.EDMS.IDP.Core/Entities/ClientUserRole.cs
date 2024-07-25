using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.IDP.Core.Entities;

[Table("ClientUserRoles")]
public class ClientUserRole
{
    private ClientUserRole()
    {

    }

    public static ClientUserRole Create(string userId, ClientRole clientRole)
    {
        return new ClientUserRole
        {
            ClientRoleId = clientRole.Id,
            UserId = userId
        };
    }

    [Key]
    public Guid Id { get; protected set; }

    /// <summary>
    /// This mapped to Id of clientRole in [ClientRoles] table
    /// </summary>
    [ForeignKey(nameof(ClientRole))]
    public Guid ClientRoleId { get; set; }

    /// <summary>
    /// This mapped to user in [AspNetUsers] table
    /// </summary>
    public string UserId { get; set; }

    public virtual ClientRole ClientRole { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
}
