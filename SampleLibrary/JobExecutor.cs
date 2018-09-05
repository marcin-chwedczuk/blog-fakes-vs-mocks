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

            _logger.LogInformation("Starting job '{jobName}' execution...", job.Name);
            
            try {
                await job.ExecuteAsync();

                _logger.LogInformation("Job '{jobName}' executed successfully...");
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Job '{jobName}' execution failed.", job.Name);
            }
        }
    }
}
