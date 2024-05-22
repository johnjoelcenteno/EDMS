using System.Runtime.Serialization;

[Serializable]
public class DbUpException : Exception
{
    public DbUpException(string message) : base(message) { }
    protected DbUpException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
}

