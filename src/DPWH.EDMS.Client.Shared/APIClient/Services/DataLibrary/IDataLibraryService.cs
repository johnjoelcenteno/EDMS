using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;

public interface IDataLibraryService
{
    Task<GetDataLibraryResultIEnumerableBaseApiResponse> GetDataLibraries();
    Task<AddDataLibraryResultBaseApiResponse> AddDataLibraries(AddDataLibraryCommand request);
    Task<UpdateDataLibraryResultBaseApiResponse> UpdateDataLibraries(UpdateDataLibraryCommand request);
    Task<DeleteDataLibraryResultBaseApiResponse> DeleteDataLibraries(Guid id);
}
