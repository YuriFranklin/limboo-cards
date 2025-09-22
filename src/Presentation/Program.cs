using LimbooCards.Application.Services;
using LimbooCards.Domain.Repositories;
using LimbooCards.Infra.Repositories;
using LimbooCards.Application.Ports;
using LimbooCards.Presentation.GraphQL.Queries;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LimbooCards.Infra.KeyValues;
using NATS.Client;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

// Discover all ObjectType classes for GraphQL
var objectTypeClasses = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectType)))
    .ToList();

// Generic HttpClient
builder.Services.AddHttpClient("generic");

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Create NATS connection
var natsUrl = Environment.GetEnvironmentVariable("NATS_URL")
    ?? throw new InvalidOperationException("Environment variable NATS_URL is not set.");
var opts = ConnectionFactory.GetDefaultOptions();
opts.Url = natsUrl;
opts.Timeout = 5000;
var cf = new ConnectionFactory();
IConnection natsConnection = cf.CreateConnection(opts);

// Register NATS connection in DI
builder.Services.AddSingleton(natsConnection);

// Register KeyValueStore using NATS connection
builder.Services.AddSingleton<IKeyValueStore>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    var logger = sp.GetRequiredService<ILogger<NatsKeyValueStore>>();
    return new NatsKeyValueStore(connection, logger);
});

// Original repositories
builder.Services.AddScoped<SubjectAutomateRepository>();
builder.Services.AddScoped<IUserRepository, UserAutomateRepository>();
builder.Services.AddScoped<ICardRepository, CardAutomateRepository>();

// Decorated repository with caching
builder.Services.AddScoped<ISubjectRepository>(sp =>
{
    var inner = sp.GetRequiredService<SubjectAutomateRepository>();
    var cache = sp.GetRequiredService<IKeyValueStore>();
    return new CachedSubjectAutomateRepository(inner, cache);
});

// Application Services
builder.Services.AddScoped<SubjectApplicationService>();
builder.Services.AddScoped<CardApplicationService>();

// GraphQL Queries
builder.Services.AddScoped<SubjectQueries>();
builder.Services.AddScoped<CardQueries>();

// Configure GraphQL server
var gqlBuilder = builder.Services
    .AddScoped<Query>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddApolloFederation()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

// Add all discovered ObjectType classes to GraphQL
foreach (var type in objectTypeClasses)
{
    gqlBuilder.AddType(type);
}

var app = builder.Build();

// Map GraphQL endpoint
app.MapGraphQL();

// Run the application
app.Run();
