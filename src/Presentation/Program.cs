
using LimbooCards.Application.Services;
using LimbooCards.Domain.Repositories;
using LimbooCards.Infra.Repositories;
using LimbooCards.Presentation.GraphQL.Queries;
using LimbooCards.Presentation.GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

var objectTypeClasses = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectType)))
    .ToList();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ISubjectRepository, SubjectAutomateRepository>();
builder.Services.AddScoped<IUserRepository, UserAutomateRepository>();
builder.Services.AddScoped<SubjectApplicationService>();

var gqlBuilder = builder.Services
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
