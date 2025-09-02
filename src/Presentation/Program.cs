
using LimbooCards.Presentation.GraphQL.Queries;
using LimbooCards.Presentation.GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<PlannerQueries>()
    .AddType<PlannerType>()
    .AddApolloFederation()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

var app = builder.Build();

app.MapGraphQL();
app.Run();
