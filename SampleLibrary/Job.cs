using System.Threading.Tasks;

namespace SampleLibrary {
    public interface Job {
        string Name { get; }
        
        Task ExecuteAsync();
    }
}