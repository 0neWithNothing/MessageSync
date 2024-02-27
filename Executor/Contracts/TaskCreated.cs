using System;

namespace Contracts
{
    public record TaskCreated
    {
        public int Number { get; init; }
        public Guid GeneratorId { get; init; }
        public int Data { get; init; }

    }
}
