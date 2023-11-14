namespace N5Challenge.Infrastructure.Elasticsearch.Interfaces;

public interface IElasticsearchService
{
    public Task IndexAsync<T>(T document, string id, CancellationToken cancellationToken = default) where T : class;

}
