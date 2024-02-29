using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Executor.Models;
using Contracts;

namespace Executor.Consumers
{
    public class GeneratorStartedConsumer :
        IConsumer<GeneratorStarted>
    {
        readonly ILogger<GeneratorStartedConsumer> _logger;
        readonly ApplicationContext _context;

        public GeneratorStartedConsumer(ILogger<GeneratorStartedConsumer> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<GeneratorStarted> context)
        {
            GeneratorDb newGenerator = new GeneratorDb { Id = context.Message.Id, Date = context.Message.Date };
            _context.Generators.Add(newGenerator);
            _context.SaveChanges();
            _logger.LogInformation($"Generator - {newGenerator.Id} added to db");
            return Task.CompletedTask;
        }
    }
}
