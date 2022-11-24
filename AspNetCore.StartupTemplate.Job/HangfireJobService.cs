using AspNetCore.StartupTemplate.Job.Jobs;
using Hangfire;

namespace AspNetCore.StartupTemplate.Job
{
    public class HangfireJobService
    {
        public static void Start()
        {
            // 每1分钟执行一次任务
            RecurringJob.AddOrUpdate<SampleJob>(nameof(SampleJob),s => s.Execute(), "0 0/1 * * * ? ", TimeZoneInfo.Local); 
        }

        public static void Stop()
        {
            RecurringJob.RemoveIfExists(nameof(SampleJob)); 
        }
        
        

    }
}