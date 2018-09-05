using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NFluent;
using Xunit;
namespace SampleLibrary.Test
{
    public class JobExecutorTest {
        private readonly string JOB_STARTED = "JOB_STARTED_FAKE_LOGGER_BOOKMARK";
        private readonly string JOB_NAME = "TestJob";

        private readonly FakeLogger<JobExecutor> _fakeLogger;
        private readonly JobExecutor _jobExecutor;

        public JobExecutorTest() {
            _fakeLogger = new FakeLogger<JobExecutor>();
            _jobExecutor = new JobExecutor(_fakeLogger);
        }

        [Fact]
        public async Task Should_log_job_starting_message_before_starting_a_job() {
            // Arrange
            var job = new ActionJob(JOB_NAME, () => {
                _fakeLogger.AddBookmark(JOB_STARTED);
            });

            // Act
            await _jobExecutor.ExecuteAsync(job);

            // Assert
            var entries =
                _fakeLogger.GetLogEntriesBeforeBookmark(JOB_STARTED);

            Check.That(entries)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Information &&
                    entry.Message.Contains(JOB_NAME));
        }
    }
}
