using MassTransit;

namespace Executor.Consumers
{
    public class GeneratorStartedConsumerDefinition :
    ConsumerDefinition<GeneratorStartedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GeneratorStartedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
