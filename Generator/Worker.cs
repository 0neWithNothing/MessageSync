using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Generator
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;
        readonly Guid _id;
        readonly Random random = new Random();
        readonly ILogger<Worker> _logger;

        public Worker(IBus bus, ILogger<Worker> logger)
        {
            _bus = bus;
            _logger = logger;
            _id = Guid.NewGuid();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Publish(new GeneratorInfoSent { Date = DateTime.UtcNow, Id = _id }, stoppingToken);

            //Console.ReadLine();
            List<TaskCreated> taskMessages = new List<TaskCreated>();

            for (int i = 1; i != 11; i++) 
            {
                TaskCreated taskMessage = new TaskCreated { Number=i, GeneratorId = _id, Data=random.Next(1, 100) };
                taskMessages.Add(taskMessage);
            }
            _logger.LogInformation(string.Join("\r\n", taskMessages.ConvertAll(t=>t.ToString())));

            while (taskMessages.Count != 0)
            {
                int index = random.Next(taskMessages.Count);
                TaskCreated taskMessage = taskMessages[index];
                await _bus.Publish(taskMessage, stoppingToken);
                int timeout = random.Next(1, 5);
                _logger.LogInformation($"Время - {DateTime.UtcNow}, номер задачи - {taskMessage.Number}, сек. до след. запроса - {timeout}");
                taskMessages.RemoveAt(index);

                await Task.Delay(timeout*1000, stoppingToken);
            }

            

        }
    }
}
