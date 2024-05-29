using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application;

public record UpdateEmployeeRequest(Guid Id, CreateUpdateEmployeeModel model) : IRequest<Guid>;
public class UpdateEmployee : IRequestHandler<UpdateEmployeeRequest, Guid>
{
    private readonly IWriteRepository _writeRepository;

    public UpdateEmployee(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var record = await _writeRepository.EmployeeRecords.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (record is null) throw new Exception("No records found");

        record.Update(
            request.model.FirstName,
            request.model.MiddleName,
            request.model.LastName,
            request.model.Office,
            request.model.Email,
            request.model.MobileNumber,
            request.model.EmployeeNumber,
            request.model.RegionCentralOffice,
            request.model.DistrictBureauService,
            request.model.Position,
            request.model.Designation,
            request.model.EmployeeId,
            request.model.Role,
            request.model.UserAccess,
            request.model.Department,
            request.model.RegionalOfficeRegion,
            request.model.RegionalOfficeProvince,
            request.model.DistrictEngineeringOffice,
            request.model.DesignationTitle
        );
        record.SetModified("Testing");

        _writeRepository.EmployeeRecords.Update(record);
        await _writeRepository.SaveChangesAsync();
        return record.Id;
    }
}
