using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozeSharp.Serivces
{
    /// <summary>
    /// Coze.cn openapi v1 会话
    /// </summary>
    public class ConversationService
    {
        private string? _baseUrl { get; set; }
        private string? _token { get; set; }
        public ConversationService(string baseurl, string token)
        {
            if(!string.IsNullOrEmpty(baseurl))
                _baseUrl = baseurl;
            if (!string.IsNullOrEmpty(token))
                _token = token;
        }
        public async Task<dynamic?> CreateAsync()
        {
            var restClient = new RestClient(_baseUrl);
            var restRequest = new RestRequest("/v1/conversation/create");
            restRequest.AddHeader("Authorization", "Bearer " + _token);
            restRequest.AddHeader("Content-Type", "application/json");
            var response = await restClient.PostAsync(restRequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
        }
    }

    public class CozeResult
    { 
        public int code { get; set; }
        public dynamic data { get; set; }
        public dynamic detail { get; set; }
        public dynamic msg { get; set; }

    }
}
