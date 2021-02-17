using MyBlog.Domain.Configurations;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.GitHub;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyBlog.Application.Authorize.Impl
{
    public class AuthorizeService : ServiceBase, IAuthorizeService
    {
        private readonly IHttpClientFactory _httpClient;

        public AuthorizeService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<ServiceResult<string>> GenerateTokenAsync(string access_token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetAccessTokenAsync(string code)
        {
            var result = new ServiceResult<string>();
            if (string.IsNullOrEmpty(code))
            {
                result.IsFailed("code为空");
                return result;
            }
            var request = new AccessTokenRequest();
            var content = new StringContent($"code={code}&client_id={request.Client_ID}&redirect_uri={request.Redirect_Uri}&client_secret={request.Client_Secret}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using var client = _httpClient.CreateClient();
            var httpResponse = await client.PostAsync(GitHubConfig.API_AccessToken, content);
            var response = await httpResponse.Content.ReadAsStringAsync();
            if (response.StartsWith("access_token"))
            {
                result.IsSuccess(response.Split("=")[1].Split("&").First());
            }
            else
            {
                result.IsFailed("code不正确");
            }
            return result;
        }

        /// <summary>
        /// 获取登录地址(Github)
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetLoginAddressAsync()
        {
            var result = new ServiceResult<string>();
            var request = new AuthorizeRequest();
            var address = string.Concat(new string[]
                {
                    GitHubConfig.API_Authorize,
                    "?client_id=",request.Client_ID,
                    "&scope=",request.Scope,
                    "&state=",request.State,
                    "&redirect_uri=",request.Redirect_Uri
                });
            result.IsSuccess(address);
            return await Task.FromResult(result);
        }
    }
}