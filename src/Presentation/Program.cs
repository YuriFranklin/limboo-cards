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
using Microsoft.Extensions.Hosting;
using LimbooCards.Domain.Services;
using LimbooCards.Infra.Services;
using LimbooCards.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// Create builder
var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

// Detect environment
var isDev = builder.Environment.IsDevelopment();

// -------------------
// Logging
// -------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(isDev ? LogLevel.Debug : LogLevel.Information);

// -------------------
// HttpClient
// -------------------
builder.Services.AddHttpClient("generic");

// -------------------
// AutoMapper
// -------------------
builder.Services.AddAutoMapper(typeof(Program));

// -------------------
// NATS connection
// -------------------
var natsUrl = Environment.GetEnvironmentVariable("NATS_URL")
    ?? throw new InvalidOperationException("Environment variable NATS_URL is not set.");

var opts = ConnectionFactory.GetDefaultOptions();
opts.Url = natsUrl;
opts.Timeout = 5000;

var cf = new ConnectionFactory();
IConnection natsConnection = cf.CreateConnection(opts);
builder.Services.AddSingleton(natsConnection);

// KeyValueStore using NATS
builder.Services.AddSingleton<IKeyValueStore>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    var logger = sp.GetRequiredService<ILogger<NatsKeyValueStore>>();
    return new NatsKeyValueStore(connection, logger);
});

// -------------------
// Repositories and services
// -------------------
builder.Services.AddMemoryCache();

builder.Services.AddScoped<SubjectAutomateRepository>();
builder.Services.AddScoped<CardAutomateRepository>();
builder.Services.AddScoped<IUserRepository, UserAutomateRepository>();
builder.Services.AddScoped<ICardRepository, CardAutomateRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPlannerRepository, PlannerDbRepository>();

builder.Services.AddScoped<ISubjectRepository>(sp =>
{
    var inner = sp.GetRequiredService<SubjectAutomateRepository>();
    var cache = sp.GetRequiredService<IKeyValueStore>();
    return new CachedSubjectAutomateRepository(inner, cache);
});

builder.Services.AddScoped<ICardRepository>(sp =>
{
    var inner = sp.GetRequiredService<CardAutomateRepository>();
    var cache = sp.GetRequiredService<IKeyValueStore>();
    return new CachedCardAuomateRepository(inner, cache);
});

builder.Services.AddHttpClient<ISynonymProvider, ConceptNetSynonymProvider>();
builder.Services.AddScoped<CardSubjectMatcherService>();

builder.Services.AddScoped<SubjectApplicationService>();
builder.Services.AddScoped<CardApplicationService>();


builder.Services.AddScoped<SubjectQueries>();
builder.Services.AddScoped<CardQueries>();

// -------------------
// GraphQL
// -------------------
// Discover all ObjectType classes
var objectTypeClasses = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectType)))
    .ToList();

var gqlBuilder = builder.Services
    .AddScoped<Query>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddApolloFederation()
    // Dev/prod configuration
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = isDev)
    .ModifyOptions(opt =>
    {
        opt.StrictValidation = !isDev;
        opt.EnableDirectiveIntrospection = isDev;
    });

// Add all discovered ObjectTypes
foreach (var type in objectTypeClasses)
{
    gqlBuilder.AddType(type);
}

// -------------------
// Application
// -------------------
var app = builder.Build();

// Map GraphQL endpoint
app.MapGraphQL();

// Run the application
app.Run();
