namespace DPWH.EDMS.Application.Models;

public class BaseApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; }

    public BaseApiResponse(T? data)
    {
        Success = true;
        Data = data;
    }

    public BaseApiResponse(bool success)
    {
        Success = success;
    }
}