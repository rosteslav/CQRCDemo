using SQRS.Core.Events;

namespace SQRS.Core.Infrastucture
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVesrion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    }
}
