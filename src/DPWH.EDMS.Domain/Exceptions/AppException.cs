using System.Runtime.Serialization;

namespace DPWH.EDMS.Domain.Exceptions;

[Serializable]
public class AppException : Exception
{
    public AppException(string message) : base(message) { }

    protected AppException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
}