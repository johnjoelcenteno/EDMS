using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Navigation.Mappers;
using DPWH.EDMS.Application.Features.RecordRequests.Mappers;
using DPWH.EDMS.Application.Models.Navigation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Navigation.Queries.GetMenuItemById;

public record GetMenuItemByIdQuery(Guid Id) : IRequest<MenuItemModel?>;

internal sealed class GetMenuItemByIdQueryHandler(IReadRepository repository) : IRequestHandler<GetMenuItemByIdQuery, MenuItemModel?>
{
    private readonly IReadRepository _repository = repository;

    public async Task<MenuItemModel?> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.MenuItemsView
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) return null;

        return MenuItemMappers.MapToModel(entity);
    }
}