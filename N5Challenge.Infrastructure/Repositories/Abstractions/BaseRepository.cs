using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities.Interfaces;
using N5Challenge.Infrastructure.Contexts;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using N5Challenge.Infrastructure.Repositories.Interfaces;

namespace N5Challenge.Infrastructure.Repositories.Abstractions;

public abstract class BaseRepository<T> : IRepository<T>
    where T : class, IEntity
{
    protected readonly N5ChallengeDbContext _dbContext;
    private readonly DbSet<T> _entitySet;
    private readonly IElasticsearchService _elasticsearchService;
    private readonly IKafkaProducer _kafkaProducer;

    private readonly List<string> _propertiesToInclude = new();

    protected BaseRepository(N5ChallengeDbContext dbContext, IElasticsearchService elasticsearchService, IKafkaProducer kafkaProducer)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(elasticsearchService);
        ArgumentNullException.ThrowIfNull(kafkaProducer);

        _dbContext = dbContext;
        _entitySet = dbContext.Set<T>();
        _elasticsearchService = elasticsearchService;
        _kafkaProducer = kafkaProducer;
    }

    protected void IncludeProperty(string property) =>
        _propertiesToInclude.Add(property);

    public async Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default) 
        => await _entitySet.Where(filter).FirstOrDefaultAsync(cancellationToken);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var queryable = _entitySet as IQueryable<T>;

        foreach(var prop in _propertiesToInclude)
            queryable = queryable.Include(prop);

        await _kafkaProducer.PublishAsync(new Kafka.DTO.KafkaEventDTO(Guid.NewGuid(), "get"), cancellationToken);
        return await queryable.ToListAsync();
    }

    public async Task<T> Add(T entity, CancellationToken cancellationToken = default)
    {
        _entitySet.Add(entity);
        return await SaveChanges(entity, "request", cancellationToken);
    }

    public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        => await _entitySet.AnyAsync(filter, cancellationToken);

    public async Task<T> Update(T entity, CancellationToken cancellationToken = default)
    {
        _entitySet.Update(entity);
        return await SaveChanges(entity, "modify", cancellationToken);
    }

    protected virtual object ParseObjectToIndex(T entity) => entity;

    private async Task<T> SaveChanges(T entity, string topic, CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync();
        
        await Task.WhenAll(
            _kafkaProducer.PublishAsync(new Kafka.DTO.KafkaEventDTO(Guid.NewGuid(), topic), cancellationToken),
            _elasticsearchService.IndexAsync(ParseObjectToIndex(entity), entity.Id.ToString(), cancellationToken)
        );
        
        return entity;
    }
}
