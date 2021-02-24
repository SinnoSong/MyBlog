﻿using MyBlog.Application.Contracts.Blog;
using MyBlog.Application.Contracts.Blog.Params;
using MyBlog.Domain.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostForAdminDto>>> QueryPostsForAdminAsync(PaginInput input)
        {
            var result = new ServiceResult<PagedList<QueryPostForAdminDto>>();

            var count = await _postRepository.GetCountAsync();

            var list = _postRepository.OrderByDescending(x => x.CreationTime)
                .PageByIndex(input.Page, input.Limit)
                .Select(x => new PostBriefForAdminDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url,
                    Year = x.CreationTime.Year,
                    CreationTime = x.CreationTime.TryToDateTime()
                }).GroupBy(x => x.Year)
                .Select(x => new QueryPostForAdminDto
                {
                    Year = x.Key,
                    Posts = x.ToList()
                }).ToList();
            result.IsSuccess(new PagedList<QueryPostForAdminDto>(count.TryToInt(), list));
            return result;
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertPostAsync(EditPostInput input)
        {
            var result = new ServiceResult();
            var post = ObjectMapper.Map<EditPostInput, Post>(input);
            post.Url = $"{post.CreationTime.ToString(" yyyy MM dd").Replace(" ", "/")}{post.Url}/";
            await _postRepository.InsertAsync(post);

            var tags = await _tagRepository.GetListAsync();

            var newTags = input.Tags.Where(item => !tags.Any(x => x.TagName.Equals(item)))
                                    .Select(item => new Tag
                                    {
                                        TagName = item,
                                        DisplayName = item
                                    });
            await _tagRepository.BulkInsertAsync(newTags);

            var postTags = input.Tags.Select(item => new PostTag
            {
                PostId = post.Id,
                TagId = _tagRepository.FirstOrDefault(x => x.TagName == item).Id
            });
            await _postTagsRepository.BulkInsertAsync(postTags);

            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Post);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdatePostAsync(int id, EditPostInput input)
        {
            var result = new ServiceResult();

            var post = await _postRepository.GetAsync(id);
            post.Title = input.Title;
            post.Author = input.Author;
            post.Url = $"{input.CreationTime.ToString(" yyyy MM dd ").Replace(" ", "/")}{input.Url}/";
            post.Html = input.Html;
            post.Markdown = input.Markdown;
            post.CreationTime = input.CreationTime;
            post.CategoryId = input.CategoryId;

            await _postRepository.UpdateAsync(post);

            var tags = await _tagRepository.GetListAsync();

            var oldPostTags = from post_tags in await _postTagsRepository.GetListAsync()
                              join tag in tags
                              on post_tags.TagId equals tag.Id
                              where post_tags.PostId.Equals(post.Id)
                              select new
                              {
                                  post_tags.Id,
                                  tag.TagName
                              };
            var removedIds = oldPostTags.Where(item => !input.Tags.Any(x => x == item.TagName) &&
                                                        tags.Any(t => t.TagName == item.TagName))
                                        .Select(item => item.Id);
            await _postTagsRepository.DeleteAsync(x => removedIds.Contains(x.Id));

            var newTags = input.Tags.Where(item => !tags.Any(x => x.TagName == item))
                                    .Select(item => new Tag
                                    {
                                        TagName = item,
                                        DisplayName = item
                                    });
            await _tagRepository.BulkInsertAsync(newTags);

            var postTags = input.Tags.Where(item => !oldPostTags.Any(x => x.TagName == item))
                                     .Select(item => new PostTag
                                     {
                                         PostId = id,
                                         TagId = _tagRepository.FirstOrDefault(x => x.TagName == item).Id
                                     });
            await _postTagsRepository.BulkInsertAsync(postTags);

            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Post);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeletePostAsync(int id)
        {
            var result = new ServiceResult();
            var post = await _postRepository.GetAsync(id);
            if (null == post)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }

            await _postRepository.DeleteAsync(id);
            await _postTagsRepository.DeleteAsync(x => x.PostId == id);

            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Post);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PostForAdminDto>> GetPostForAdminAsync(int id)
        {
            var result = new ServiceResult<PostForAdminDto>();

            var post = await _postRepository.GetAsync(id);

            var tags = from post_tags in await _postTagsRepository.GetListAsync()
                       join tag in await _tagRepository.GetListAsync()
                       on post_tags.TagId equals tag.Id
                       where post_tags.PostId.Equals(post.Id)
                       select tag.TagName;
            var detail = ObjectMapper.Map<Post, PostForAdminDto>(post);
            detail.Tags = tags;
            detail.Url = post.Url.Split("/").Where(x => !string.IsNullOrEmpty(x)).Last();

            result.IsSuccess(detail);
            return result;
        }
    }
}