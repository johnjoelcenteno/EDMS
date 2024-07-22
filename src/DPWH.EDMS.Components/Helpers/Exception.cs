using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Components.Helpers;

[Serializable]
public class AppException : Exception
{
    public AppException(string message) : base(message) { }

    protected AppException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
}
