using System.Runtime.Serialization;

namespace Application.Exceptions;

[Serializable]
public class BadRequestException : BookRetailCaseStudyException
{
    protected BadRequestException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public BadRequestException() : base() { }

    public BadRequestException(string message)
       : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}