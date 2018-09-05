using System;
using Microsoft.Extensions.Logging;

namespace SampleLibrary.Test {
    public class LogEntry {
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public LogEntry(LogLevel logLevel, string message, Exception ex = null) {
            LogLevel = logLevel;
            Message = message;
            Exception = ex;
        }

        public override string ToString()
            => $"{LogLevel}: {Message}" + 
               (Exception != null ? $" Exception: {Exception.GetType().Name}" : "") +
               ".";
    }   
}