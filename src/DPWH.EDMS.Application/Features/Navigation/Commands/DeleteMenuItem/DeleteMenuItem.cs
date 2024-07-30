using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Navigation.Commands.DeleteMenuItem;

internal class DeleteMenuItem
{
}

public record class DeleteMenuItemCommand(Guid Id) : IRequest<Guid>;
internal sealed class DeleteMenuItemCommandHandler(IWriteRepository writeRepository) : IRequestHandler<DeleteMenuItemCommand, Guid>
{
    public async Task<Guid> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = await writeRepository.MenuItems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (menuItem is null) return request.Id;

        writeRepository.MenuItems.Remove(menuItem);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
