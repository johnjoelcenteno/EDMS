namespace DPWH.EDMS.Application.Features.Users.Commands.DeactivateUser;

public record DeactivateUserResult
{
    public required string UserId { get; set; }
    public string? OldAccess { get; set; }
    public string? NewAccess { get; set; }
}