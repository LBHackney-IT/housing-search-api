using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using System;

namespace HousingSearchApi.V1.Infrastructure.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void ConfigureElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            var url = configuration.GetValue<string>("ELASTICSEARCH_DOMAIN_URL");
            if (string.IsNullOrEmpty(url))
                url = "https://localhost:9200";

            var pool = new SingleNodeConnectionPool(new Uri(url));

            var connectionSettings =
                new ConnectionSettings(pool)
                    .PrettyJson()
                    .ThrowExceptions()
                    .DisableDirectStreaming()
#if DEBUG
                    .EnableDebugMode()
                    .ServerCertificateValidationCallback((o, cert, chain, sslErrors) => true)
#endif
                    ;
            var esClient = new ElasticClient(connectionSettings);

            services.TryAddSingleton<IElasticClient>(esClient);
        }
    }
}
