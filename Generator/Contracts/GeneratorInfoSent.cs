using System;

namespace Contracts
{
    public record GeneratorInfoSent
    {
        public DateTime Date { get; init; }
        public Guid Id { get; init; }
    }
}