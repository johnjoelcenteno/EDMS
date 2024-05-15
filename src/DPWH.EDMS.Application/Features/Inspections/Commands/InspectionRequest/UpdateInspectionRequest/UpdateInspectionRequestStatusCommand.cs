using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateInspectionRequest;

public record UpdateInspectionRequestStatusCommand : IRequest<UpdateInspectionRequestResult>
{
    public Guid Id { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
}
internal class UpdateInspectionRequestStatusHandler : IRequestHandler<UpdateInspectionRequestStatusCommand, UpdateInspectionRequestResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateInspectionRequestStatusHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateInspectionRequestResult> Handle(UpdateInspectionRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var inspection = _repository.InspectionRequests.FirstOrDefault(x => x.Id == request.Id)
                ?? throw new AppException("No inspection found");

        var status = (InspectionRequestStatus)Enum.Parse(typeof(InspectionRequestStatus), request.Status);

        inspection.UpdateStatus(status, _principal.GetUserName());

        _repository.InspectionRequests.Update(inspection);

        await _repository.SaveChangesAsync(cancellationToken);
        return new UpdateInspectionRequestResult(inspection);
    }
}
