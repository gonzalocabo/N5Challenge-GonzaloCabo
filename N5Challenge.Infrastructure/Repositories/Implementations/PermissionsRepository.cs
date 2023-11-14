using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities.Permissions;
using N5Challenge.Infrastructure.Contexts;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using N5Challenge.Infrastructure.Repositories.Abstractions;
using N5Challenge.Infrastructure.Repositories.Interfaces;

namespace N5Challenge.Infrastructure.Repositories.Implementations;

public class PermissionsRepository : BaseRepository<Permission>, IPermissionsRepository
{
    public PermissionsRepository(N5ChallengeDbContext dbContext, IElasticsearchService elasticsearchService, IKafkaProducer kafkaProducer) 
        : base(dbContext, elasticsearchService, kafkaProducer)
    {
        IncludeProperty("PermissionType");
    }

    protected override object ParseObjectToIndex(Permission entity)
    {
        return new
        {
            entity.Id,
            entity.EmployeeSurname,
            entity.EmployeeForename,
            entity.PermissionDate,
            PermissionType = entity.PermissionTypeId
        };
    }
}
