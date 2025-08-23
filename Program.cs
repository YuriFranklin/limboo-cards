using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var assembly = Assembly.GetExecutingAssembly();
var serviceTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "LimbooCards.Services");

foreach (var type in serviceTypes)
{
    builder.Services.AddScoped(type);
}

builder.Services
    .AddGraphQLServer()
    .AddApolloFederation();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

app.Run();