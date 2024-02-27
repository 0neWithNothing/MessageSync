using System.Threading.Tasks;
using MassTransit;
using Contracts;
using Microsoft.Extensions.Logging;
using Executor.Models;
using System.Linq;
using System;

namespace Executor.Consumers
{
    public class TaskCreatedConsumer :
        IConsumer<TaskCreated>,
        IConsumer<GeneratorInfoSent>
    {
        readonly ILogger<TaskCreatedConsumer> _logger;
        readonly ApplicationContext _context;

        public TaskCreatedConsumer(ILogger<TaskCreatedConsumer> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<GeneratorInfoSent> context)
        {
            GeneratorDb newGenerator = new GeneratorDb { Id = context.Message.Id, Date = context.Message.Date };
            _context.Generators.Add(newGenerator);
            _context.SaveChanges();
            _logger.LogInformation($"Generator - {newGenerator.Id} added to db");
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<TaskCreated> context)
        {
            var gen = _context.Generators.FirstOrDefault(g=>g.Id == context.Message.GeneratorId);
            TaskDb newTask = new TaskDb { Generator=gen, Number=context.Message.Number, Data=context.Message.Data };
            _context.Tasks.Add(newTask);
            _context.SaveChanges();

            var generatorsCount = _context.Generators.Count();
            var tasks = _context.Tasks.Where(t => t.Number == newTask.Number && !t.IsCopmleted);


            if (tasks.Count() == generatorsCount)
            {
                int result = 0;

                foreach (var task in tasks)
                {
                    result += task.Data;
                    task.IsCopmleted = true;
                }
                _logger.LogInformation($"Время - {DateTime.UtcNow}, номер задачи - {newTask.Number}, сумма - {result}");
                _context.SaveChanges();
            }

            var tasksCount = _context.Tasks.Where(t => t.IsCopmleted).Count();

            if (tasksCount == generatorsCount * 10)
            {
                _logger.LogInformation("Все 10 задач были выполнены");
            }

            return Task.CompletedTask;
        }
    }
}
