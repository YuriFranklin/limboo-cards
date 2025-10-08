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
using LimbooCards.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LimbooCards.Presentation.GraphQL.Mutations;
using LimbooCards.Infra.Settings;

// Create builder
var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

// Detect environment
var isDev = builder.Environment.IsDevelopment();

// -------------------
// Settings
// -------------------
builder.Services.Configure<NatsSettings>(
    builder.Configuration.GetSection("Nats"));

builder.Services.Configure<CardSettings>(
    builder.Configuration.GetSection("Services:Card"));

builder.Services.Configure<SubjectSettings>(
    builder.Configuration.GetSection("Services:Subject"));

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
builder.Services.AddSingleton<IConnection>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<NatsSettings>>().Value;
    var opts = ConnectionFactory.GetDefaultOptions();
    opts.Url = settings.Url;
    opts.Timeout = 5000;
    var cf = new ConnectionFactory();
    return cf.CreateConnection(opts);
});

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

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

builder.Services.AddScoped<CardSubjectMatcherService>();

builder.Services.AddScoped<PlannerApplicationService>();
builder.Services.AddScoped<SubjectApplicationService>();
builder.Services.AddScoped<CardApplicationService>();

builder.Services.AddScoped<PlannerQueries>();
builder.Services.AddScoped<SubjectQueries>();
builder.Services.AddScoped<CardQueries>();

builder.Services.AddScoped<PlannerMutations>();
builder.Services.AddScoped<CardMutations>();
builder.Services.AddScoped<SubjectMutations>();
// -------------------
// GraphQL
// -------------------
// Discover all ObjectType classes
var objectTypeClasses = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectType)))
    .ToList();

var gqlBuilder = builder.Services
    .AddScoped<Query>()
    .AddScoped<Mutation>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
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
