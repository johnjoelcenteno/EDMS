using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.IDP.Core.Entities;

[Table("ClientRoles")]
public class ClientRole
{
    private ClientRole()
    {

    }

    public static ClientRole Create(int clientId, string clientName, Guid roleId, string roleName)
    {
        var clientRole = new ClientRole
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            ClientName = clientName,
            RoleId = roleId,
            RoleName = roleName
        };

        return clientRole;
    }

    [Key]
    public Guid Id { get; protected set; }
    /// <summary>
    /// This mapped to Id of client in [Clients] table
    /// </summary>
    public int ClientId { get; set; }
    /// <summary>
    /// This mapped to ClientId
    /// </summary>
    public string ClientName { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
