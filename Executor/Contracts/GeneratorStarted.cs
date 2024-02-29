using System;

namespace Contracts
{
    public record GeneratorStarted
    {
        public DateTime Date { get; init; }
        public Guid Id { get; init; }
    }
}