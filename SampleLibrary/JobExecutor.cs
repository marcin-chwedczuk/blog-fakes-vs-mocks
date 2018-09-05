using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SampleLibrary {
    public class JobExecutor {
        private readonly ILogger<JobExecutor> _logger;

        public JobExecutor(ILogger<JobExecutor> logger) {
            _logger = logger;
        }

        public async Task ExecuteAsync(Job job) {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            // _logger.LogDebug($"Start time {DateTime.UtcNow}");
            _logger.LogInformation("Starting job '{jobName}' execution...", job.Name);

            try {
                await job.ExecuteAsync();

                _logger.LogInformation("Job '{jobName}' executed successfully...", job.Name);
                // _logger.LogDebug($"Stop time {DateTime.UtcNow}");
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Job '{jobName}' execution failed.", job.Name);
            }
        }
    }
}
