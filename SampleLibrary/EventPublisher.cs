using System.Threading.Tasks;

namespace SampleLibrary {
    public interface EventPublisher {
        Task Publish(Event @event);
    }

    public interface Event { }
}