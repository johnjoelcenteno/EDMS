using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Components.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<GetUserByIdResult, UserModel>();
    }
}