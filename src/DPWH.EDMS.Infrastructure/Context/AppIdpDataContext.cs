using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Users.Queries;
using DPWH.EDMS.IDP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Infrastructure.Context;

public class AppIdpDataContext : IdentityDbContext<ApplicationUser>, IReadAppIdpRepository
{
    public AppIdpDataContext(DbContextOptions<AppIdpDataContext> options) : base(options)
    {

    }

    public IQueryable<ApplicationUser> UsersView => Users.AsNoTracking();
    public IQueryable<IdentityUserClaim<string>> UserClaimsView => UserClaims.AsNoTracking();
    public required DbSet<ViewUserAccess> ViewUserAccess { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ViewUserAccess>()
            .HasNoKey()
            .ToView("ViewUserAccess");

        builder.Entity<UserBasicInfo>()
            .ToTable("AspNetUsers");

        builder.Entity<EmployeeInfo>()
            .ToTable("AspNetUsers");

        builder.Entity<ApplicationUser>(b =>
        {
            b.HasOne(u => u.UserBasicInfo)
                .WithOne()
                .HasForeignKey<UserBasicInfo>(u => u.Id);
            b.Navigation(u => u.UserBasicInfo);

            b.HasOne(u => u.EmployeeInfo)
                .WithOne()
                .HasForeignKey<EmployeeInfo>(u => u.Id);
            b.Navigation(u => u.EmployeeInfo);
        });

        base.OnModelCreating(builder);
    }

}