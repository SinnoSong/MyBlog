using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using MyBolg.BackgroundJobs.Jobs.Hangfire;
using MyBolg.BackgroundJobs.Jobs.HotNews;
using System;

namespace MyBolg.BackgroundJobs
{
    public static class MyBlogBackgroundJobsExtensions
    {
        public static void UseHangfireTest(this IServiceProvider service)
        {
            var job = service.GetService<HangfireTestJob>();

            RecurringJob.AddOrUpdate("定时任务测试", () => job.ExecuteAsync(), CronType.Minute());
        }

        /// <summary>
        /// 每日热点数据抓取
        /// </summary>
        /// <param name="service"></param>
        public static void UseHotNewsJob(this IServiceProvider service)
        {
            var job = service.GetService<HotNewsJob>();

            RecurringJob.AddOrUpdate("每日热点数据抓取", () => job.ExecuteAsync(), CronType.Hour(1, 2));
        }
    }
}