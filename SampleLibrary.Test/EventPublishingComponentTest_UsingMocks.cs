using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using Xunit;

namespace SampleLibrary.Test {
    public class EventPublishingComponentTest_UsingMocks {
        private readonly EventPublisher _eventPublisher;
        private readonly EventPublishingComponent _component;

        public EventPublishingComponentTest_UsingMocks() {
            _eventPublisher = Substitute.For<EventPublisher>();
            _component = new EventPublishingComponent(_eventPublisher);
        }

        [Fact]
        public async Task Should_publish_FirstEvent() {
            // Arrange
            FirstEvent firstEvent = null;
            await _eventPublisher
                .Publish(Arg.Do<FirstEvent>(e => firstEvent = e));

            // Act
            await _component.Publish();

            // Assert
            await _eventPublisher.Received(1)
                .Publish(Arg.Any<FirstEvent>());

            Check.That(firstEvent)
                .IsNotNull();

            Check.That(firstEvent.Id)
                .IsNotZero();
        }

        [Fact]
        public async Task Should_publish_SecondEvent() {
            // Arrange
            SecondEvent secondEvent = null;
            await _eventPublisher
                .Publish(Arg.Do<SecondEvent>(e => secondEvent = e));

            // Act
            await _component.Publish();

            // Assert
            await _eventPublisher.Received(1)
                .Publish(Arg.Any<SecondEvent>());

            Check.That(secondEvent)
                .IsNotNull();

            Check.That(secondEvent.Id)
                .Not.IsNullOrWhiteSpace();
        }
    }
}