﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyBlog.Domain.HotNews.Repositories
{
    /// <summary>
    /// IHotNewsRepository
    /// </summary>
    public interface IHotNewsRepository : IRepository<HotNews, Guid>
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="hotNews"></param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<HotNews> hotNews);
    }
}