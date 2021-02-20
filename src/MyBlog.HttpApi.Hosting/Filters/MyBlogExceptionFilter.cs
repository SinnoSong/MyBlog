using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.HttpApi.Hosting.Filters
{
    public class MyBlogExceptionFilter : IExceptionFilter
    {
        private readonly ILog _log;

        public MyBlogExceptionFilter()
        {
            _log = LogManager.GetLogger(typeof(MyBlogExceptionFilter));
        }

        public void OnException(ExceptionContext context)
        {
            _log.Error($"{context.HttpContext.Request.Path}|{context.Exception.Message}", context.Exception);
        }
    }
}