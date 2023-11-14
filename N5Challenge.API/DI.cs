using Microsoft.EntityFrameworkCore;
using Serilog;
using N5Challenge.Application.DependencyInjection;
using N5Challenge.Infrastructure.Contexts;
using N5Challenge.Infrastructure.Elasticsearch;
using N5Challenge.Infrastructure.Elasticsearch.Implementations;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using N5Challenge.Infrastructure.Kafka;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using N5Challenge.Infrastructure.Repositories.DependencyInjection;
using N5Challenge.Infrastructure.UnitOfWork;

namespace N5Challenge.API;

public static class DI
{
    public static void InjectDependencies(this WebApplicationBuilder builder)
    {        
        builder.Services.AddDbContext<N5ChallengeDbContext>((services, options) =>
        {
            var connstring = services.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
            options.UseSqlServer(connstring);
        });

        builder.Services
            .AddHttpContextAccessor()
            .ConfigureMediatR()
            .ConfigureRepos()
            .ConfigureUnitOfWork()
            .AddSingleton<IElasticsearchService, ElasticsearchService>()
            .AddSingleton<IKafkaProducer, KafkaProducer>()
            .AddSingleton<Serilog.ILogger>(new LoggerConfiguration().WriteTo.Console().CreateLogger())
            .Configure<ElasticsearchOptions>(builder.Configuration.GetSection("Elasticsearch"))
            .Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));
    }
}
