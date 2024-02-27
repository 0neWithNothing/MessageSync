using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;

namespace Executor
{
    public class Program
    {
        static bool? _isRunningInContainer;

        static bool IsRunningInContainer =>
            _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
   
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
                        x.AddDelayedMessageScheduler();

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
                            if (IsRunningInContainer)
                                cfg.Host("rabbitmq");

                            cfg.UseDelayedMessageScheduler();
                            
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    services.AddDbContext<ApplicationContext>();
                    services.AddTransient<DbInitialiser>();
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                });
    }
}
