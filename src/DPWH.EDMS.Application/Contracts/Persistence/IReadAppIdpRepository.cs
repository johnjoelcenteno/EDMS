using DPWH.EDMS.Application.Features.Users.Queries;
using DPWH.EDMS.IDP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Contracts.Persistence;

public interface IReadAppIdpRepository
{
    IQueryable<ApplicationUser> UsersView { get; }
    IQueryable<IdentityUserClaim<string>> UserClaimsView { get; }
    DbSet<ViewUserAccess> ViewUserAccess { get; }
}