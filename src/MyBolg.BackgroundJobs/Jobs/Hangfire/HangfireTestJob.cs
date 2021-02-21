using System;
using System.Threading.Tasks;

namespace MyBolg.BackgroundJobs.Jobs.Hangfire
{
    public class HangfireTestJob : IBackgroundJob
    {
        public async Task ExecuteAsync()
        {
            Console.WriteLine("定时任务测试");
            await Task.CompletedTask;
        }
    }
}