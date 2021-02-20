using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.DependencyInjection;

namespace MyBlog.Application.Caching
{
    public class MyBlogApplicationCachingServiceBase : ITransientDependency
    {
        public IDistributedCache Cache { get; set; }
    }
}