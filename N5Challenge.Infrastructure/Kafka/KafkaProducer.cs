using Confluent.Kafka;
using Microsoft.Extensions.Options;
using N5Challenge.Infrastructure.Kafka.DTO;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace N5Challenge.Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly string _url;
    private readonly string _topic;
    private readonly Serilog.ILogger _logger;

    public KafkaProducer(IOptions<KafkaOptions> options, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNull(logger);

        ArgumentException.ThrowIfNullOrEmpty(options.Value.Url);
        ArgumentException.ThrowIfNullOrEmpty(options.Value.Topic);

        _url = options.Value.Url;
        _topic = options.Value.Topic;
        _logger = logger;
    }

    public async Task PublishAsync(KafkaEventDTO eventDto, CancellationToken cancellationToken = default)
    {
        try
        {
            ProducerConfig config = new()
            {
                BootstrapServers = _url
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var message = new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(eventDto)
                };

                var result = await producer.ProduceAsync(_topic, message, cancellationToken);

                return;
            }
        }
        catch(Exception ex) 
        {
            _logger.Error(ex, "Error while publishing to Kafka");
            throw;
        }

    }
}
