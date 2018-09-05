using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NFluent;
using Xunit;
namespace SampleLibrary.Test
{
    public class JobExecutorTest {
        private readonly string JOB_STARTED = "JOB_STARTED_FAKELOGGER_BOOKMARK";
        private readonly string JOB_FINISHED = "JOB_FINISHED_FAKELOGGER_BOOKMARK";
        private readonly string JOB_NAME = "TestJob";

        private readonly FakeLogger<JobExecutor> _fakeLogger;
        private readonly Job _bookmarkingJob;
        
        private readonly JobExecutor _jobExecutor;


        public JobExecutorTest() {
            _fakeLogger = new FakeLogger<JobExecutor>();

            _bookmarkingJob = new ActionJob(JOB_NAME, () => {
                _fakeLogger.AddBookmark(JOB_STARTED);
                _fakeLogger.AddBookmark(JOB_FINISHED);
            });

            _jobExecutor = new JobExecutor(_fakeLogger);
        }

        [Fact]
        public async Task Should_log__job_is_starting__message_before_starting_a_job() {
            // Act
            await _jobExecutor.ExecuteAsync(_bookmarkingJob);

            // Assert
            var entries =
                _fakeLogger.GetLogEntriesBeforeBookmark(JOB_STARTED);

            Check.That(entries)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Information &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME, "starting"));
        }

        [Fact]
        public async Task Should_log__job_executed__message_after_job_execution() {
            // Act
            await _jobExecutor.ExecuteAsync(_bookmarkingJob);

            // Assert
            var entries =
                _fakeLogger.GetLogEntriesAfterBookmark(JOB_FINISHED);

            Check.That(entries)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Information &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME, "executed"));
        }

        [Fact]
        public async Task Should_log_exception_thrown_by_job() {
            // Arrange
            var JOB_FAILURE = "JOB_FAILURE_BOOKMARK";
            var EXCEPTION_MESSAGE = "some-message";

            var exceptionThrowingJob = new ActionJob(JOB_NAME, () => {
                _fakeLogger.AddBookmark(JOB_FAILURE);
                throw new NotImplementedException(EXCEPTION_MESSAGE);
            });

            // Act
            await _jobExecutor.ExecuteAsync(exceptionThrowingJob);

            // Assert
            var entries =
                _fakeLogger.GetLogEntriesAfterBookmark(JOB_FAILURE);

            Check.That(entries)
                .HasElementThatMatches(entry => 
                    entry.LogLevel == LogLevel.Error &&
                    entry.Message.ContainsAllCaseInsensitive(JOB_NAME) &&
                    entry.Exception?.Message == EXCEPTION_MESSAGE);
        }
    }
}
