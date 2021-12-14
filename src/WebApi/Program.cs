
var seed = args.Any(x => x == "/seed");
if (seed) args = args.Except(new[] { "/seed" }).ToArray();

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureSerilog()
    ;

builder.Services.AddMvcCore(options => { options.EnableEndpointRouting = false; })
    .AddAuthorization()
    ;

Startup.ConfigureApplication(builder);
Startup.ConfigureDependencies(builder);

var app = builder.Build();

Startup.Configure(app);

if (seed)
{
    SeedData.EnsureSeedData(app.Services);
    return;
}

app.Run();

