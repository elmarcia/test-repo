using OpsCrew.Continuity.Api.CompositionRoot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCoreApi(builder.Configuration);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/api/health"));

app.Run();
