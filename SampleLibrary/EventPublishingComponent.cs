using System.Threading.Tasks;

namespace SampleLibrary {
    public class EventPublishingComponent {
        private readonly EventPublisher _eventPublisher;

        public EventPublishingComponent(EventPublisher eventPublisher)
            => _eventPublisher = eventPublisher;

        public async Task Publish() {
            await _eventPublisher.Publish(new FirstEvent(3));
            await _eventPublisher.Publish(new SecondEvent("ZDKAH-JXI7"));
        }
    }
    
    public class FirstEvent : Event {
        public int Id { get; }

        public FirstEvent(int id)
            => Id = id;
    }

    public class SecondEvent : Event {
        public string Id { get; }

        public SecondEvent(string id)
            => Id = id;
    }
}