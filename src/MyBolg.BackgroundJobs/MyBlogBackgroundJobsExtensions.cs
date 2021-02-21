﻿using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using MyBolg.BackgroundJobs.Jobs.Hangfire;
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
    }
}