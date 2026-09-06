using SoccerManager.API.Extensions;
using SoccerManager.API.Middleware;
using SoccerManager.Application;
using SoccerManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithBearerAuth();

builder.Services.AddConfiguredCors(builder.Configuration);
builder.Services.AddKeycloakAuthentication(builder.Configuration);
builder.Services.AddCurrentUser();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase();

// First in the pipeline so it also covers failures raised by CORS and authentication.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
