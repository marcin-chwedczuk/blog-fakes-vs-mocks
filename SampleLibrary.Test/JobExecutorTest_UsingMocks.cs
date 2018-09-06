using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using NFluent;
using NSubstitute;
using Xunit;

namespace SampleLibrary.Test {
    public class JobExecutorTest_UsingMocks {
        private readonly string JOB_NAME = "TestJob";

        private readonly Job _job;
        private readonly List<LogEntry> _loggedEntires = new List<LogEntry>();

        private readonly JobExecutor _jobExecutor;
        
        public JobExecutorTest_UsingMocks() {
            _job = Substitute.For<Job>();
            _job.Name.Returns(JOB_NAME);
            
            var logger = Substitute.For<ILogger<JobExecutor>>();

            logger
                // pitfall: using `x => x.Log(0, 0, 0, null, null)` will not work
                .WhenForAnyArgs(x => x.Log(0, 0, null, null, null))
                .Do(call => {
                    var logLevel = call.ArgAt<LogLevel>(0);
                    var state = call.ArgAt<object>(2);
                    var exeption = call.ArgAt<Exception>(3);
                    dynamic formatter = call.ArgAt<object>(4);

                    var logEntry = new LogEntry(logLevel, formatter(state, exeption), exeption);
                    _loggedEntires.Add(logEntry);
                });

            _jobExecutor = new JobExecutor(logger);
        }

        [Fact]
        public async Task Should_log__job_is_starting__message_before_starting_a_job() {
            // Arrange
            var loggedBeforeJobStart = new List<LogEntry>();

            _job
                .When(x => x.ExecuteAsync())
                .Do(call => loggedBeforeJobStart.AddRange(_loggedEntires));

            // Act
            await _jobExecutor.ExecuteAsync(_job);

            // Assert
            Check.That(loggedBeforeJobStart)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Information &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME, "starting"));
        }

        [Fact]
        public async Task Should_log__job_executed__message_after_job_execution() {
            // Arrange
            _job
                .When(x => x.ExecuteAsync())
                .Do(call => _loggedEntires.Clear());

            // Act
            await _jobExecutor.ExecuteAsync(_job);

            // Assert
            Check.That(_loggedEntires)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Information &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME, "executed"));
        }

        [Fact]
        public async Task Should_log_exception_thrown_by_job() {
            // Arrange
            var EXCEPTION_MESSAGE = "some-message";

            _job
                .When(x => x.ExecuteAsync())
                .Do(call => {
                    _loggedEntires.Clear();
                    throw new NotImplementedException(EXCEPTION_MESSAGE); 
                });

            // Act
            await _jobExecutor.ExecuteAsync(_job);

            // Assert
            Check.That(_loggedEntires)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Error &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME) &&
                    entry.Exception?.Message == EXCEPTION_MESSAGE);
        }
    }
}