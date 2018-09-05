using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SampleLibrary.Test
{
    public partial class FakeLogger<T> : ILogger<T> {
        private readonly List<LogEntry> _loggedEntries = new List<LogEntry>();
        private readonly Dictionary<string, int> _bookmarks = new Dictionary<string, int>();

        public IReadOnlyList<LogEntry> LoggedEntries 
            => _loggedEntries;

        #region ILogger
        
        public IDisposable BeginScope<TState>(TState state) {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter) 
        {
            _loggedEntries.Add(new LogEntry(logLevel, formatter(state, exception), exception));
        }
        
        #endregion

        public IReadOnlyList<LogEntry> TakeSnapshot() {
            return new List<LogEntry>(LoggedEntries);
        }

        public void AddBookmark(string name) {
            _bookmarks.Add(name, _loggedEntries.Count);
        }

        public IReadOnlyList<LogEntry> GetLogEntriesBeforeBookmark(string name) {
            int numberOfEntriesBeforeBookmark = _bookmarks[name];

            return LoggedEntries
                .Take(numberOfEntriesBeforeBookmark)
                .ToList();
        }

        public IReadOnlyList<LogEntry> GetLogEntriesAfterBookmark(string name) {
            int numberOfEntriesBeforeBookmark = _bookmarks[name];

            return LoggedEntries
                .Skip(numberOfEntriesBeforeBookmark)
                .ToList();
        }       
    }
}