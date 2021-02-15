using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Shared
{
    public class MyBlogDbConsts
    {
        public static class DbTableName
        {
            public const string Posts = "Posts";
            public const string Categories = "Categories";
            public const string Tags = "Tags";
            public const string PostTags = "PostTags";
            public const string FriendLinks = "FriendLinks";
        }
    }
}