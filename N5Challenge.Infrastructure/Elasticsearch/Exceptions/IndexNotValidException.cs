namespace N5Challenge.Infrastructure.Elasticsearch.Exceptions;

internal class IndexNotValidException : Exception
{
    public string DebugInformation { get; }
    public IndexNotValidException(string message, string debugInformation) : base(message)
    {
        DebugInformation = debugInformation;
    }
}
