using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleLibrary.Test {
    public class InMemoryEventPublisher : EventPublisher {
        private readonly List<Event> _publishedEvents = new List<Event>();

        public IReadOnlyList<Event> PublishedEvents
            => _publishedEvents;

        public Task Publish(Event @event) {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            _publishedEvents.Add(@event);
            return Task.CompletedTask;
        }
    }
}