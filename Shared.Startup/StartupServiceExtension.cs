using Core.Application.Services.Concretes;
using Core.Application.Services.Contracts;
using Core.Infrastructure.Domain.Entities;
using Core.Infrastructure.Persistences.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Sample.Api.Infrastructure;

namespace Shared.Startup
{
    public static class StartupServiceExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(a =>
            {
                var mongoClient = new MongoClient(configuration.GetConnectionString("Mongo"));
                return mongoClient.GetDatabase("CurrencyRate");
            });

            services.AddSingleton<IMongoBaseRepository<CurrencyRate>>(a =>
            {
                var database = a.GetService<IMongoDatabase>();
                return new MongoBaseRepository<CurrencyRate>(database, 
                    configuration.GetSection("CurrencyRateCollectionName").Value);
            });

            // Register Guid serializer for make it possible to convert id from string to Guid and vice versa
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            services.AddScoped<ICurrencyRateService, CurrencyRateService>();

            services.AddHostedService<TimedHostedService>();
        }
    }
}