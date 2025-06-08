using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozeSharp.Protocols;
using CozeSharp.Services;
using CozeSharp.Utils;

namespace CozeSharp
{
    public class CozeAgent
    {
        private string _apiUrl { get; set; } = "https://api.coze.cn";
        private string _wsUrl { get; set; } = "wss://ws.coze.cn";
        private Services.Chat.ChatService? _chatService = null;
        private Services.AudioWaveService? _audioService =null;
        private Services.AudioOpusService _audioOpusService = new Services.AudioOpusService();

        #region 属性
        /// <summary>
        /// 个人令牌
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// 智能体Id
        /// </summary>
        public string BotId { get; set; } = string.Empty;
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// 会话Id
        /// </summary>
        public string? ConversionId { get; set; }
        #endregion

        #region 事件
        public delegate Task MessageEventHandler(string type, string message);
        public event MessageEventHandler? OnMessageEvent = null;

        public delegate Task AudioEventHandler(byte[] opus);
        public event AudioEventHandler? OnAudioEvent = null;
        #endregion

        #region 构造函数
        public CozeAgent() { }
        public CozeAgent(string token, string botId, string userId, string? conversionId = null)
        {
            Token = token;
            BotId = botId;
            UserId = userId;
            ConversionId = conversionId;
        }
        #endregion

        public async Task Start() {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(BotId) || string.IsNullOrEmpty(UserId))
            {
                LogConsole.ErrorLine($"扣子 启动失败 - 令牌、智能体Id或用户Id不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(ConversionId))
            {
                Serivces.Conversation.ConversationService conversationService = new Serivces.Conversation.ConversationService(_apiUrl, Token);
                ConversionId = conversationService.CreateAsync();
                if (string.IsNullOrEmpty(ConversionId))
                {
                    LogConsole.ErrorLine($"扣子 启动失败 - 获取会话失败");
                    return;
                }
                else
                {
                    LogConsole.InfoLine($"扣子 创建会话成功 - " + ConversionId);
                }
            }

            _chatService = new Services.Chat.ChatService(_wsUrl, Token, BotId);
            _chatService.ConversionId = ConversionId;
            _chatService.UserId = UserId;
            _chatService.UserLanguage = "zh"; // 设置用户语言为中文
            _chatService.OnMessageEvent += ChatService_OnMessageEvent;
            _chatService.OnAudioEvent += ChatService_OnAudioEvent;
            _chatService.Start();

            if (Global.IsAudio)
            {
                _audioService = new AudioWaveService();
                _audioService.OnPcmAudioEvent += AudioService_OnPcmAudioEvent;
            }
        }

        private async Task AudioService_OnPcmAudioEvent(byte[] pcm)
        {
            if (_chatService != null)
                await _chatService.SendAudio_PCM_AppendAsync(pcm);
        }

        private async Task ChatService_OnAudioEvent(byte[] opus)
        {
            if (_audioService != null)
            {
                byte[] pcmData = _audioOpusService.ConvertOpusToPcm(opus);
                _audioService.AddOutSamples(pcmData);
            }

            if(OnAudioEvent!=null)
                await OnAudioEvent(opus);
        }

        private async Task ChatService_OnMessageEvent(string type, string message)
        {
            if (OnMessageEvent != null)
                await OnMessageEvent(type, message);
        }

        /// <summary>
        /// 打断消息
        /// </summary>
        /// <returns></returns>
        public async Task ChatAbort()
        {
            if (_chatService != null)
                await _chatService.ChatAbort();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ChatMessage(string message)
        {
            if (_chatService != null)
                await _chatService.ChatMessage(message);
        }
        /// <summary>
        /// 开始录音
        /// </summary>
        public async Task StartRecording()
        {
            if (_audioService != null && _chatService != null)
            {
                await _chatService.SendChat_UpdateAsync();
                _audioService.StartRecording();
            }
        }
        /// <summary>
        /// 结束录音
        /// </summary>
        public async Task StopRecording()
        {
            if (_audioService != null && _chatService != null)
            {
                _audioService.StopRecording();
                await _chatService.SendAudio_Buffer_CompleteAsync();
            }
        }
    }
}
