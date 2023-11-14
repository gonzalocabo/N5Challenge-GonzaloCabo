using N5Challenge.Infrastructure.Kafka.DTO;

namespace N5Challenge.Infrastructure.Kafka.Interfaces;

public interface IKafkaProducer
{
    public Task PublishAsync(KafkaEventDTO eventDto, CancellationToken cancellationToken = default);
}
