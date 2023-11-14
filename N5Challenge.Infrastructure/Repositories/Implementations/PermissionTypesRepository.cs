using N5Challenge.Domain.Entities.Permissions;
using N5Challenge.Infrastructure.Contexts;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using N5Challenge.Infrastructure.Repositories.Abstractions;
using N5Challenge.Infrastructure.Repositories.Interfaces;

namespace N5Challenge.Infrastructure.Repositories.Implementations;

public class PermissionTypesRepository : BaseRepository<PermissionType>, IPermissionTypesRepository
{
    public PermissionTypesRepository(N5ChallengeDbContext dbContext, IElasticsearchService elasticsearchService, IKafkaProducer kafkaProducer) 
        : base(dbContext, elasticsearchService, kafkaProducer)
    {
    }
}
