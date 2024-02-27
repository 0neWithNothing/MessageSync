using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Executor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var initialiser = services.GetRequiredService<DbInitialiser>();

            initialiser.Run();

            await app.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    services.AddDbContext<ApplicationContext>();
                    services.AddTransient<DbInitialiser>();
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                });
    }
}
