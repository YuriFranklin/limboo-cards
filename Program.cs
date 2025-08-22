using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

var assembly = Assembly.GetExecutingAssembly();
var serviceTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "LimbooCards.Services");

foreach (var type in serviceTypes)
{
    builder.Services.AddScoped(type);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();