using System.Runtime.Serialization;

namespace Application.Exceptions;

[Serializable]
public class BookRetailCaseStudyException : Exception
{
    protected BookRetailCaseStudyException(SerializationInfo info,
     StreamingContext context) : base(info, context)
    {
    }
    public BookRetailCaseStudyException() : base() { }

    public BookRetailCaseStudyException(string message)
       : base(message)
    {
    }

    public BookRetailCaseStudyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}