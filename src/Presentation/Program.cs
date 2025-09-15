
using LimbooCards.Application.Services;
using LimbooCards.Domain.Repositories;
using LimbooCards.Infra.Repositories;
using LimbooCards.Presentation.GraphQL.Queries;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

var objectTypeClasses = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectType)))
    .ToList();


builder.Services.AddHttpClient("generic");
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ISubjectRepository, SubjectAutomateRepository>();
builder.Services.AddScoped<IUserRepository, UserAutomateRepository>();
builder.Services.AddScoped<SubjectApplicationService>();
builder.Services.AddScoped<SubjectQueries>();

builder.Services.AddScoped<ICardRepository, CardAutomateRepository>();
builder.Services.AddScoped<CardApplicationService>();
builder.Services.AddScoped<CardQueries>();

var gqlBuilder = builder.Services
    .AddScoped<Query>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddApolloFederation()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

foreach (var type in objectTypeClasses)
{
    gqlBuilder.AddType(type);
}

var app = builder.Build();

app.MapGraphQL();
app.Run();
