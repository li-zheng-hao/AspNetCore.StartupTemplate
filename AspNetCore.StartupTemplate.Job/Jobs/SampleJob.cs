using AspNetCore.StartUpTemplate.IService;
using Microsoft.Extensions.Logging;
using Quickwire.Attributes;

namespace AspNetCore.StartupTemplate.Job.Jobs
{
    [RegisterService]
    [InjectAllInitOnlyProperties]
    public class SampleJob
    {
        public ILogger<SampleJob> _logger { get; init; }
        public IUserService UserService { get; init; }
        public async System.Threading.Tasks.Task Execute()
        {
            _logger.LogInformation($"SampleJob is running {DateTime.Now}");
        }
    }
}
