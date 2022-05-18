using MyLab.ApiClient;
using MyLab.RabbitClient;
using MyLab.Search.Searcher.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var srv = builder.Services;

srv.AddControllers();

srv.AddApiClients(reg =>
    {
        reg.RegisterContract<ISearcherApiV3>();
    }, builder.Configuration)
    .AddRabbit(RabbitConnectionStrategy.Background);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
