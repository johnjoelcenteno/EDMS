namespace DPWH.EDMS.Application.Models;

public abstract class BaseResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }

    public BaseResponse()
    {
        Success = true;
    }

    public BaseResponse(bool success, string? error)
    {
        Success = success;
        Error = error;
    }
}

public abstract class IdResponse : BaseResponse
{
    public Guid Id { get; set; }

    public IdResponse(Guid id) : base()
    {
        Id = id;
    }
}

public sealed class CreateResponse : IdResponse
{
    public CreateResponse(Guid id) : base(id) { }
}

public class DeleteResponse : IdResponse
{
    public DeleteResponse(Guid id) : base(id) { }
}

public class UpdateResponse : IdResponse
{
    public UpdateResponse(Guid id) : base(id) { }
}

