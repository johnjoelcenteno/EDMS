using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.Navigation;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Navigation.Commands.UpdateMenuItem;

public record class UpdateMenuItemRequest(Guid Id, UpdateMenuItemModel model) : IRequest<Guid?>;

public class UpdateMenuItem : IRequestHandler<UpdateMenuItemRequest, Guid?>
{
    public UpdateMenuItem(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        WriteRepository = writeRepository;
        _principal = principal;
    }

    public IWriteRepository WriteRepository { get; }

    private readonly ClaimsPrincipal _principal;

    public async Task<Guid?> Handle(UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var menuItem = WriteRepository.MenuItems.FirstOrDefault(x => x.Id == request.Id);
        if (menuItem is null) return null;
        string modifiedBy = _principal.GetUserName();

        var model = request.model;
        menuItem.Update(
            model.Text,
            model.Url,
            model.Icon,
            model.Expanded,
            model.Level,
            model.SortOrder,
            model.NavType,
            model.AuthorizedRoles,
            model.ParentId,
            modifiedBy
        );

        await WriteRepository.SaveChangesAsync(cancellationToken);
        return menuItem.Id;
    }
}