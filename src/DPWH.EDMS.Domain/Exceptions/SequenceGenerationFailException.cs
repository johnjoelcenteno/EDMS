using System.Runtime.Serialization;

namespace DPWH.EDMS.Domain.Exceptions;

[Serializable]
public class SequenceGenerationFailException : Exception
{
    public SequenceGenerationFailException(string property) : base($"Unable to generate code for {property}") { }

    protected SequenceGenerationFailException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
        base(serializationInfo, streamingContext)
    { }

    public static void ThrowIfNull(object? argument, string propertyName)
    {
        if (argument is null)
        {
            throw new SequenceGenerationFailException(propertyName);
        }
    }
}