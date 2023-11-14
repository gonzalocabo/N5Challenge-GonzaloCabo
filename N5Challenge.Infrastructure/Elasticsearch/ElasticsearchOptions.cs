namespace N5Challenge.Infrastructure.Elasticsearch;

public class ElasticsearchOptions
{
    public string Url { get; set; } = string.Empty;
    public string DefaultIndex { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
