using CozeSharp.Utils;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozeSharp.Serivces.Conversation
{
    public class ConversationService
    {
        private string TAG = "扣子";
        private string _apiUrl { get; set; } = "https://api.coze.cn";
        private string _token { get; set; } = string.Empty;
        public ConversationService(string apiUrl,string token) { 
            _apiUrl = apiUrl;
            _token = token;
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <returns>会话Id</returns>
        public string CreateAsync()
        {
            var restClient = new RestClient(_apiUrl);
            var restRequest = new RestRequest("/v1/conversation/create");
            restRequest.AddHeader("Authorization", "Bearer " + _token);
            restRequest.AddHeader("Content-Type", "application/json");
            var response = restClient.PostAsync(restRequest);
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogConsole.ErrorLine($"{TAG} 创建会话失败 - " + response.Result.StatusCode.ToString() + " - " + response.Result.Content);
                return string.Empty;
            }
            
            string conversionId = string.Empty;//conversation.data.id;
            try
            {
                if(string.IsNullOrEmpty(response.Result.Content))
                    return conversionId;

                var json = JObject.Parse(response.Result.Content);
                if (json["code"]?.ToString() == "0")
                {
                    conversionId = json["data"]?["id"]?.ToString() ?? string.Empty;
                }
            }
            catch{}
            return conversionId;
        }
    }
}
