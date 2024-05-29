using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain;
using MediatR;

namespace DPWH.EDMS.Application;

public record class CreateEmployeeRequest(CreateUpdateEmployeeModel model) : IRequest<Guid>;
public class CreateEmployee : IRequestHandler<CreateEmployeeRequest, Guid>
{
    private readonly IWriteRepository _writeRepository;

    public CreateEmployee(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        EmployeeRecord record = CreateUpdateEmployeeMappers.MapModelToEntity(request.model);
        record.SetCreated("Admin");
        _writeRepository.EmployeeRecords.Add(record);
        await _writeRepository.SaveChangesAsync();
        return record.Id;
    }
}
