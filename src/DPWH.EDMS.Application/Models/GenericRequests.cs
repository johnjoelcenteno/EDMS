using MediatR;

namespace DPWH.EDMS.Application.Models;

public class QueryPaged<TResponse> : IRequest<TResponse> where TResponse : BaseResponse
{
    public int PageNumber { get; }
    public int ItemsPerPage { get; }

    public QueryPaged(int pageNumber, int itemsPerPage)
    {
        PageNumber = pageNumber;
        ItemsPerPage = itemsPerPage;
    }
}

public class Create<TModel, TResponse> : IRequest<TResponse> where TResponse : BaseResponse
{
    public TModel Model { get; }

    public Create(TModel model)
    {
        Model = model;
    }
}

public class UpdateWithId<TModel, TResponse> : IRequest<TResponse> where TResponse : BaseResponse
{
    public Guid Id { get; }
    public TModel Model { get; }

    public UpdateWithId(Guid id, TModel model)
    {
        Id = id;
        Model = model;
    }
}

public class DeleteWithId<TModel, TResponse> : IRequest<TResponse> where TResponse : BaseResponse
{
    public Guid Id { get; }

    public DeleteWithId(Guid id)
    {
        Id = id;
    }
}