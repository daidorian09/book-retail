using System.Runtime.Serialization;

namespace Application.Exceptions;

[Serializable]
public class UnauthorizedException : BookRetailCaseStudyException
{
    protected UnauthorizedException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public UnauthorizedException()
        : base()
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}