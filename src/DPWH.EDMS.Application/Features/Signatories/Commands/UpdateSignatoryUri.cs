using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.Signatories.Commands
{
    public record UpdateSignatoryUriRequest(Guid SignatoryId, string Uri) : IRequest<Guid>;
    public class UpdateSignatoryUri : IRequestHandler<UpdateSignatoryUriRequest, Guid>
    {
        private readonly IWriteRepository _writeRepository;

        public UpdateSignatoryUri(IWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }
        public async Task<Guid> Handle(UpdateSignatoryUriRequest request, CancellationToken cancellationToken)
        {
            var signatory = _writeRepository.Signatories.FirstOrDefault(x => x.Id == request.SignatoryId);
            signatory.UpdateUriSignature(request.Uri);
            await _writeRepository.SaveChangesAsync();
            return signatory.Id;
        }
    }
}
