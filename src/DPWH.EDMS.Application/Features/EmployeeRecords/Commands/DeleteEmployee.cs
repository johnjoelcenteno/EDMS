using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application;

public record class DeleteEmployeeRequest(Guid Id) : IRequest<Guid>;
public class DeleteEmployee : IRequestHandler<DeleteEmployeeRequest, Guid>
{
    private readonly IWriteRepository _writeRepository;

    public DeleteEmployee(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid> Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        var record = await _writeRepository.EmployeeRecords.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (record is null) throw new Exception("No records found");
        _writeRepository.EmployeeRecords.Remove(record);
        _writeRepository.SaveChangesAsync();

        return record.Id;
    }
}
