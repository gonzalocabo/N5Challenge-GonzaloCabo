namespace N5Challenge.Application.Permission.Exceptions;

public class StatusCodeException : Exception
{
    public int StatusCode { get; }
    public ErrorMessageObject ErrorMessageObject { get; }
    public StatusCodeException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
        ErrorMessageObject = new ErrorMessageObject(message);
    }
}

public record ErrorMessageObject(string error);