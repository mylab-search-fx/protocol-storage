using MyLab.ApiClient;
using MyLab.HttpMetrics;
using MyLab.Log;
using MyLab.RabbitClient;
using MyLab.Search.Searcher.Client;
using MyLab.WebErrors;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var srv = builder.Services;

srv.AddControllers(opts => opts.AddExceptionProcessing());

srv.AddApiClients(reg =>
    {
        reg.RegisterContract<ISearcherApiV3>();
    }, builder.Configuration)
    .AddRabbit(RabbitConnectionStrategy.Background)
    .AddRabbitTracing()
    .AddLogging(l => l.AddMyLabConsole())
    .AddUrlBasedHttpMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseHttpMetrics();

app.MapControllers();
app.MapMetrics();

app.Run();
