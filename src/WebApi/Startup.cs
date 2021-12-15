using CosmosDb.Model;
using CosmosDb.Model.Configuration;
using CosmosDb.Model.Interop;
using CosmosDb.Utils;
using CosmosDb.Utils.Interop;
using MaskedEmails.Inbox.Http;
using MaskedEmails.Services;
using MaskedEmails.Services.Configuration;
using MaskedEmails.Services.Interop;
using MaskedEmails.Services.Storage;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Utils;
using Utils.Interop;
using WebApi.Configuration;
using WebApi.Owin;
using WebApi.Services;
using WebApi.Services.Interop;

public static class Startup
{
    public static void Configure(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.HandleExceptions();
        app.UseAuthentication();
        app.UseClaimsIdentifier();

        app.UseStaticFiles();

        app.UseMvc();
        app.UseSpaStaticFiles();
        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
            //if (Environment.IsDevelopment())
            //    spa.UseAngularCliServer(npmScript: "start");
        });
    }

    public static void ConfigureApplication(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var appSettings = new AppSettings();
        var appSettingsSection = configuration.GetSection("AppSettings");
        appSettingsSection.Bind(appSettings);

        var httpSettings = new HttpSettings();
        var httpSettingsSection = configuration.GetSection("HttpSettings");
        httpSettingsSection.Bind(httpSettings);

        var cosmosDbSettings = new CosmosDbSettings();
        var cosmosDbSettingsSection = configuration.GetSection("CosmosDb");
        cosmosDbSettingsSection.Bind(cosmosDbSettings);

        services.AddOptions();
        services.Configure<AppSettings>(appSettingsSection);
        services.Configure<HttpSettings>(httpSettingsSection);
        services.Configure<CosmosDbSettings>(cosmosDbSettingsSection);

        services.UseInboxApi(configuration);

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = appSettings.Authority;
                options.RequireHttpsMetadata = appSettings.RequireHttpsMetadata;
                options.Audience = appSettings.Audience;
            });

        services.AddMvcCore(options => { options.EnableEndpointRouting = false; })
            .AddAuthorization()
            ;

        if (!builder.Environment.IsDevelopment())
        {
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = httpSettings.HttpsPort;
            });
        }

        // this is required for serving the following static files
        // https://angular/assets/auth.clientConfiguration.json
        // https://angular/assets/auth.clientConfiguration.prod.json
        // https://angular/assets/static/silent-renew.html

        services.AddSpaStaticFiles(options =>
        {
            options.RootPath = "ClientApp/dist";
        });
    }

    public static void ConfigureDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // CosmosDb

        services.AddSingleton<ICosmosDbClientFactory, CosmosDbClientFactory>();
        services.AddTransient<ICosmosDbContext, CosmosDbContext>();
        services.AddTransient<ICosmosOperations>(provider =>
        {
            var clientFactory = provider.GetRequiredService<ICosmosDbClientFactory>();
            var client = clientFactory.CreateClient();
            var logger = provider.GetRequiredService<ILogger<CosmosRequestChargeOperations>>();

            return new CosmosRequestChargeOperations(client, logger);
        });

        services.AddTransient<IMaskedEmailService, CosmosDbMaskedEmailService>();
        services.AddTransient<IProfilesService, CosmosDbProfilesService>();

        services.AddTransient<IUniqueIdGenerator, UniqueIdGenerator>();

        services.AddTransient<ISequence, StorageTableSequence>(
            provider => new StorageTableSequence(configuration));

        services.AddSingleton<IMaskedEmailCommandService, MaskedEmailCommandQueueService>(
            provider => new MaskedEmailCommandQueueService(configuration));
    }
}

static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .WriteTo.File("App_Data/IdentityServer4_log.txt")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
        });

        return builder;
    }
}

