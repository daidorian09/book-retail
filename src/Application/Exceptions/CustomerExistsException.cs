using System.Runtime.Serialization;

namespace Application.Exceptions;

[Serializable]
public class CustomerExistsException : BookRetailCaseStudyException
{
    protected CustomerExistsException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public CustomerExistsException() : base() { }

    public CustomerExistsException(string message)
       : base(message)
    {
    }

    public CustomerExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}