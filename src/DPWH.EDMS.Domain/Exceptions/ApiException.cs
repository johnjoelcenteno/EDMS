using System.Net;
using System.Runtime.Serialization;

namespace DPWH.EDMS.Domain.Exceptions;

[Serializable]
public class ApiException : Exception
{
    public ApiException(string? message) : base(message) { }

    public ApiException(string? message, Exception inner) : base(message, inner) { }

    public ApiException(string? message, HttpStatusCode statusCode, string content) : base(message)
    {
        StatusCode = statusCode;
        Content = content;
    }

    protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext) { }

    public HttpStatusCode StatusCode { get; }
    public string? Content { get; }
}