using System.Linq;
using System.Threading.Tasks;
using NFluent;
using Xunit;

namespace SampleLibrary.Test {
    public class EventPublishingComponentTest_UsingFakes {
        private readonly InMemoryEventPublisher _eventPublisher;
        private readonly EventPublishingComponent _component;

        public EventPublishingComponentTest_UsingFakes() {
            _eventPublisher = new InMemoryEventPublisher();
            _component = new EventPublishingComponent(_eventPublisher);
        }

        [Fact]
        public async Task Should_publish_FirstEvent() {
            // Act
            await _component.Publish();

            // Assert
            var firstEvent = _eventPublisher.PublishedEvents
                .OfType<FirstEvent>()
                .SingleOrDefault();

            Check.That(firstEvent)
                .IsNotNull();

            Check.That(firstEvent.Id)
                .IsNotZero();
        }

        [Fact]
        public async Task Should_publish_SecondEvent() {
            // Act
            await _component.Publish();

            // Assert
            var secondEvent = _eventPublisher.PublishedEvents
                .OfType<SecondEvent>()
                .SingleOrDefault();

            Check.That(secondEvent)
                .IsNotNull();

            Check.That(secondEvent.Id)
                .Not.IsNullOrWhiteSpace();
        }
    }
}