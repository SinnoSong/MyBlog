﻿using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog.Impl
{
    public partial class BlogService
    {
        #region Posts

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
            var post = await _postRepository.FindAsync(id);
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

        #endregion Posts

        #region Categories

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteCategoryAsync(int id)
        {
            var result = new ServiceResult();
            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("id", id));
                return result;
            }
            await _categoryRepository.DeleteAsync(category);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Category);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertCategoryAsync(EditCategoryInput input)
        {
            var result = new ServiceResult();
            var category = ObjectMapper.Map<EditCategoryInput, Category>(input);
            await _categoryRepository.InsertAsync(category);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_FriendLink);
            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryCategoryForAdminDto>>> QueryCategoryForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryCategoryForAdminDto>>();

            var posts = await _postRepository.GetListAsync();

            var categories = _categoryRepository.GetListAsync().Result
                                          .Select(x => new QueryCategoryForAdminDto
                                          {
                                              Id = x.Id,
                                              CategoryName = x.CategoryName,
                                              DisplayName = x.DisplayName,
                                              Count = posts.Count(p => p.CategoryId == x.Id)
                                          });
            result.IsSuccess(categories);
            return result;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateCategoryAsync(int id, EditCategoryInput input)
        {
            var result = new ServiceResult();
            var category = await _categoryRepository.GetAsync(id);
            ObjectMapper.Map(input, category);
            await _categoryRepository.UpdateAsync(category);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_FriendLink);
            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        #endregion Categories

        #region Tags

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteTagAsync(int id)
        {
            var result = new ServiceResult();
            var tag = await _tagRepository.FindAsync(id);
            if (tag == null)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }
            await _tagRepository.DeleteAsync(tag);
            await _postTagsRepository.DeleteAsync(x => x.TagId == id);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Tag);
            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertTagAsync(EditTagInput input)
        {
            var result = new ServiceResult();
            var tag = ObjectMapper.Map<EditTagInput, Tag>(input);
            await _tagRepository.InsertAsync(tag);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Tag);
            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryTagForAdminDto>>> QueryTagsForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryTagForAdminDto>>();
            var postTags = await _postTagsRepository.GetListAsync();
            var tags = _tagRepository.GetListAsync().Result.Select(x => new QueryTagForAdminDto
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                TagName = x.TagName,
                Count = postTags.Count(p => p.TagId == x.Id)
            });
            result.IsSuccess(tags);
            return result;
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateTagAsync(int id, EditTagInput input)
        {
            var result = new ServiceResult();
            var tag = await _tagRepository.FindAsync(id);
            if (tag == null)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }
            ObjectMapper.Map(input, tag);
            await _tagRepository.UpdateAsync(tag);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_Tag);
            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        #endregion Tags

        #region FriendLinks

        /// <summary>
        /// 删除友链
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteFriendLinkAsync(int id)
        {
            var result = new ServiceResult();
            var friendLink = await _friendLinksRepository.FindAsync(id);
            if (friendLink == null)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("id", id));
                return result;
            }
            await _friendLinksRepository.DeleteAsync(id);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_FriendLink);
            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 新增友链
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertFriendLinkAsync(EditFriendLinkInput input)
        {
            var result = new ServiceResult();
            var friendLink = ObjectMapper.Map<EditFriendLinkInput, FriendLink>(input);
            await _friendLinksRepository.InsertAsync(friendLink);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_FriendLink);
            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 查询友链列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryFriendLinkForAdminDto>>> QueryFriendLinkForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryFriendLinkForAdminDto>>();
            var friendLinks = await _friendLinksRepository.GetListAsync();
            var dto = ObjectMapper.Map<List<FriendLink>, IEnumerable<QueryFriendLinkForAdminDto>>(friendLinks);
            result.IsSuccess(dto);
            return result;
        }

        /// <summary>
        /// 更新友链
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateFriendLinkAsync(int id, EditFriendLinkInput input)
        {
            var result = new ServiceResult();
            var friendLink = await _friendLinksRepository.FindAsync(id);
            if (friendLink == null)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("id", id));
                return result;
            }
            ObjectMapper.Map(input, friendLink);
            await _friendLinksRepository.UpdateAsync(friendLink);
            await _blogCacheService.RemoveAsync(CachePrefix.Blog_FriendLink);
            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        #endregion FriendLinks
    }
}