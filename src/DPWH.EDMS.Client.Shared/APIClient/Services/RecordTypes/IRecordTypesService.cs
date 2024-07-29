using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;

public interface IRecordTypesService
{
    Task<GuidBaseApiResponse> CreateRecordTypesAsync(CreateRecordTypeModel model);

    Task<QueryRecordTypesModelBaseApiResponse> QueryByIdRecordTypesAsync(Guid id);

    Task<QueryRecordTypesModelListBaseApiResponse> QueryByCategoryRecordTypesAsync(string category);

    Task<DataSourceResult> QueryRecordTypesAsync(DataSourceRequest request);

    Task<GuidNullableBaseApiResponse> UpdateRecordTypesAsync(Guid id,UpdateRecordTypeModel model);

    Task<DeleteResponse> DeleteRecordTypesAsync(Guid id);
}

