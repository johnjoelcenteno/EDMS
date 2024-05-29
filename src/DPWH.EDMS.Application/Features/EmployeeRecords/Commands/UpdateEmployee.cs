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
        record.FirstName = request.model.FirstName;
        record.MiddleName = request.model.MiddleName;
        record.LastName = request.model.LastName;
        record.Office = request.model.Office;
        record.Email = request.model.Email;
        record.MobileNumber = request.model.MobileNumber;
        record.EmployeeNumber = request.model.EmployeeNumber;
        record.RegionCentralOffice = request.model.RegionCentralOffice;
        record.DistrictBureauService = request.model.DistrictBureauService;
        record.Position = request.model.Position;
        record.Designation = request.model.Designation;
        record.SetModified("Testing");

        _writeRepository.EmployeeRecords.Update(record);
        await _writeRepository.SaveChangesAsync();
        return record.Id;
    }
}
