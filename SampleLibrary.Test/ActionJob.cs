using System;
using System.Threading.Tasks;

namespace SampleLibrary.Test {
    public class ActionJob : Job {
        private readonly Action _action;

        public string Name { get; }

        public ActionJob(string name, Action action) {
            Name = name;
            _action = action;
        }

        public Task ExecuteAsync() {
            try {
                _action();
                return Task.CompletedTask;
            }
            catch(Exception ex) {
                return Task.FromException(ex);
            }
        }
    }
}