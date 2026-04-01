using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Services.Data;
using TA_API.Services.TaskItems;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));

    // Register application services
    builder.Services.AddScoped<ITaskItemService, TaskItemService>();

    // Add CORS policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:1234")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Comment out AddControllers/MapControllers if you prefer to implement Minimal APIs.
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    // Apply EF Core migrations automatically on startup
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AssessmentDbContext>();
        db.Database.Migrate();
    }
    
    app.UseSerilogRequestLogging();

    // Use CORS policy
    app.UseCors("AllowFrontend");

    app.MapGet("/", () => "Technical Assessment API");
    app.MapGet("/lbhealth", () => "Technical Assessment API");

    app.MapControllers();
}

app.Run();
