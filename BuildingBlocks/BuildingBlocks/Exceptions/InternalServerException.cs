

namespace BuildingBlocks.Exceptions;

public class InternalServerException: Exception
{
    public InternalServerException()
        : base("An internal server error occurred.")
    {
    }

    public InternalServerException(string message)
        : base(message)
    {
        Detail = message;
    }

    public InternalServerException(string message, Exception innerException)
        : base(message, innerException)
    {
        Detail= innerException.InnerException?.Message ?? innerException.Message;
    }

    public string? Detail { get; private set; }
}

