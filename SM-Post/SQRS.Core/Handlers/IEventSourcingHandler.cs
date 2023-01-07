using SQRS.Core.Domain;

namespace SQRS.Core.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregateRoot);
        Task<T> GetByIdAsync(Guid aggregateId);
    }
}
