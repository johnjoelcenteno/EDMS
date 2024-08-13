using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.Signatories.Queries
{
    public record GetByIdRequest(Guid SignatoriesId) : IRequest<QuerySignatoryModel?>;
    public class GetById : IRequestHandler<GetByIdRequest, QuerySignatoryModel?>
    {
        private readonly IReadRepository _readRepository;

        public GetById(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }
        public Task<QuerySignatoryModel?> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var signatory = _readRepository.SignatoriesView.FirstOrDefault(x => x.Id == request.SignatoriesId);
            if (signatory is null)
            {
                return null;
            }

            return Task.FromResult(SignatoryMappers.Map(signatory));
        }
    }
}
