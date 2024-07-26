using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;

public interface IDataLibraryService
{
    Task<DataSourceResult> GetDataLibraries(DataSourceRequest body);
    Task<AddDataLibraryResultBaseApiResponse> AddDataLibraries(AddDataLibraryCommand request);
    Task<UpdateDataLibraryResultBaseApiResponse> UpdateDataLibraries(UpdateDataLibraryCommand request);
    Task<DeleteDataLibraryResultBaseApiResponse> DeleteDataLibraries(Guid id);
}
