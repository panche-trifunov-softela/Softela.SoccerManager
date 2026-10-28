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
builder.Services.AddApiRateLimiting();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// No request body this API accepts comes close to 1 MB; a larger one is refused with 413.
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1_048_576);

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

// After authentication, so a signed-in caller is counted by user id rather than by IP.
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
