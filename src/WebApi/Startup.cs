using CosmosDb.Model;
using CosmosDb.Model.Configuration;
using CosmosDb.Model.Interop;
using CosmosDb.Utils;
using CosmosDb.Utils.Interop;
using MaskedEmails.Services;
using MaskedEmails.Services.Configuration;
using MaskedEmails.Services.Interop;
using MaskedEmails.Services.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utils;
using Utils.Interop;
using WebApi.Configuration;
using WebApi.Owin;
using WebApi.Services;
using WebApi.Services.Interop;

namespace WebApi
{
    public class Startup
    {
        IWebHostEnvironment Environment { get; }
        IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options => { options.EnableEndpointRouting = false; })
                .AddAuthorization()
                ;

            ConfigureApplication(services);
            ConfigureDependencies(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!Environment.IsDevelopment())
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
                if (Environment.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });
        }

        private void ConfigureApplication(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            appSettingsSection.Bind(appSettings);

            var httpSettings = new HttpSettings();
            var httpSettingsSection = Configuration.GetSection("HttpSettings");
            httpSettingsSection.Bind(httpSettings);

            var cosmosDbSettings = new CosmosDbSettings();
            var cosmosDbSettingsSection = Configuration.GetSection("CosmosDb");
            cosmosDbSettingsSection.Bind(cosmosDbSettings);

            services.AddOptions();
            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<HttpSettings>(httpSettingsSection);
            services.Configure<CosmosDbSettings>(cosmosDbSettingsSection);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = appSettings.Authority;
                    options.RequireHttpsMetadata = appSettings.RequireHttpsMetadata;
                    options.Audience = appSettings.Audience;
                });

            if (!Environment.IsDevelopment())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = httpSettings.HttpsPort;
                });
            }

            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = "ClientApp/dist";
            });
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            // CosmosDb

            services.AddSingleton<ICosmosDbClientFactory, CosmosDbClientFactory>();
            services.AddTransient<ICosmosDbContext, CosmosDbContext>();
            services.AddTransient<ICosmosOperations>(provider => {
                var clientFactory = provider.GetRequiredService<ICosmosDbClientFactory>();
                var client = clientFactory.CreateClient();
                var logger = provider.GetRequiredService<ILogger<CosmosRequestChargeOperations>>();

                return new CosmosRequestChargeOperations(client, logger);
            });

            services.AddTransient<IMaskedEmailService, CosmosDbMaskedEmailService>();
            services.AddTransient<IProfilesService, CosmosDbProfilesService>();

            services.AddTransient<IUniqueIdGenerator, UniqueIdGenerator>();

            services.AddTransient<ISequence, StorageTableSequence>(
                provider => new StorageTableSequence(Configuration));

            services.AddSingleton<IMaskedEmailCommandService, MaskedEmailCommandQueueService>(
                provider => new MaskedEmailCommandQueueService(Configuration));
        }
    }
}
