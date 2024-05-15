using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.IDP.Core.Entities;

public class UserActivityLog
{
    private UserActivityLog(
        string username,
        string application,
        string activity,
        string status,
        string ipAddress,
        string userAgent)
    {
        Username = username;
        Application = application;
        Activity = activity;
        Status = status;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Date = DateTimeOffset.Now;
    }

    public static UserActivityLog Create(
        string username,
        string application,
        string activity,
        string status,
        string ipAddress,
        string userAgent)
    {
        return new UserActivityLog(username, application, activity, status, ipAddress, userAgent);
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public string Username { get; private set; }
    public string Application { get; private set; }
    public string Activity { get; private set; }
    public string Status { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
}