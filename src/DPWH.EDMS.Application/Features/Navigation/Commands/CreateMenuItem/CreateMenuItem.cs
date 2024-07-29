using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.Navigation;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Navigation.Commands.CreateMenuItem;

public record class CreateMenuItemRequest(CreateMenuItemModel Model) : IRequest<Guid>;
internal sealed class CreateMenuItemHandler(IWriteRepository WriteRepository, ClaimsPrincipal _principal) : IRequestHandler<CreateMenuItemRequest, Guid>
{

    public async Task<Guid> Handle(CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        //if (model.Code is null && model.Category == RecordTypesCategory.PersonalRecords.GetDescription() || model.Code is not null && model.Category != RecordTypesCategory.PersonalRecords.GetDescription())
        //{
        //    throw new AppException($"Only Personal Records are required to have a Document Code.");
        //}

        string createdBy = _principal.GetUserName();

        MenuItem menuItemMapping = MenuItem.Create(
            model.Text,
            model.Url,
            model.Icon,
            model.Expanded,
            model.Level,
            model.SortOrder,
            model.NavType,
            model.AuthorizedRoles,
            model.ParentId,
            createdBy
        );

        WriteRepository.MenuItems.Add(menuItemMapping);
        await WriteRepository.SaveChangesAsync();
        return menuItemMapping.Id;
    }
}
