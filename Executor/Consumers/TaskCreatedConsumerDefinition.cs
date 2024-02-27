using MassTransit;

namespace Executor.Consumers
{
    public class TaskCreatedConsumerDefinition :
    ConsumerDefinition<TaskCreatedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskCreatedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
