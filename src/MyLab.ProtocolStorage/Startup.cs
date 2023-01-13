using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLab.ApiClient;
using MyLab.HttpMetrics;
using MyLab.Log;
using MyLab.RabbitClient;
using MyLab.Search.SearcherClient;
using MyLab.Search.SearcherClient;
using MyLab.WebErrors;
using Prometheus;

namespace MyLab.ProtocolStorage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers(opts => opts.AddExceptionProcessing())
                .AddNewtonsoftJson();

            services.AddApiClients(reg =>
                {
                    reg.RegisterContract<ISearcherApiV3>();
                }, Configuration)
                .AddRabbit(RabbitConnectionStrategy.Background)
                .ConfigureRabbit(Configuration)
                .AddRabbitTracing()
                .AddLogging(l => l.AddMyLabConsole())
                .AddUrlBasedHttpMetrics();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}
