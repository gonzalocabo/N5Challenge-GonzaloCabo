using Microsoft.Extensions.Options;
using N5Challenge.Infrastructure.Elasticsearch.Exceptions;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using Nest;

namespace N5Challenge.Infrastructure.Elasticsearch.Implementations;

public class ElasticsearchService : IElasticsearchService
{
    private readonly ElasticClient _client;
    private readonly Serilog.ILogger _logger;

    public ElasticsearchService(IOptions<ElasticsearchOptions> options, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNull(logger);

        ArgumentException.ThrowIfNullOrEmpty(options.Value.Url);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.DefaultIndex);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.User);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.Password);
        
        var values = options.Value;

        _client = new ElasticClient(new ConnectionSettings(new Uri(values.Url))
            .DefaultIndex(values.DefaultIndex)
            .ServerCertificateValidationCallback((x,y,z,q) => true)
            .BasicAuthentication(values.User, values.Password));

        _logger = logger;
    }

    public async Task IndexAsync<T>(T document, string id, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var response = await _client.IndexAsync(document, 
                index => index
                .Index(_client.ConnectionSettings.DefaultIndex)
                .Id(id), 
                cancellationToken);

            if (!response.IsValid)
                throw new IndexNotValidException($"Failed indexing document id {id}, more information at DebugInformation property", 
                    response.DebugInformation);
        }
        catch(Exception ex) 
        {
            _logger.Error(ex, "Error while indexing document");
        }
    }
}
