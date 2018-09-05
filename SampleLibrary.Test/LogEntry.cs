using Microsoft.Extensions.Logging;

namespace SampleLibrary.Test {
    public class LogEntry {
        public LogLevel LogLevel { get; }
        public string Message { get; }

        public LogEntry(LogLevel logLevel, string message) {
            LogLevel = logLevel;
            Message = message;
        }

        public override string ToString()
            => $"{LogLevel}: {Message}";
    }   
}