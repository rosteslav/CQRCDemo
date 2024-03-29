﻿using Post.Cmd.Domain.Aggregates;
using SQRS.Core.Domain;
using SQRS.Core.Events;
using SQRS.Core.Exceptions;
using SQRS.Core.Infrastucture;
using SQRS.Core.Producers;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer) 
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStram = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (eventStram == null || !eventStram.Any()) 
            {
                throw new AggregateNotFoudException($"Aggregate for Id {aggregateId} not found");
            }

            return eventStram.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVesrion)
        {
            var eventStram = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVesrion != -1 && eventStram[^1].Version != expectedVesrion)
            {
                throw new ConcurencyException();
            }

            var version = expectedVesrion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}
