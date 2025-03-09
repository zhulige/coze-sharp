using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozeSharp.Protocols;
using CozeSharp.Serivces;

namespace CozeSharp
{
    public class CozeAgent
    {
        public string TOKEN { get; set; } = "pat_OpwStwaNPCwHxtpcfcNisstwE6ncRZoaUjqxOjSyECf2CaMbvhu57377QABqshjV";
        public string WEB_BASE_URL { get; set; } = "https://api.coze.cn";
        public string WEB_SOCKET_URL { get; set; } = "wss://ws.coze.cn";
        public string BOT_ID { get; set; } = "7475209051979350070";
        public string USER_ID { get; set; } = "朱利戈";

        public delegate void MessageEventHandler(string message);
        public delegate void AudioEventHandler(byte[] opus);
        public event MessageEventHandler? OnMessageEvent = null;
        public event AudioEventHandler? OnAudioEvent = null;

        string? ConversionId = null;

        private ConversationService? _conversationService = null;
        private WebSocketService? _webSocketService = null;
        private AudioService? _audioService = null;

        public CozeAgent()
        {
            
        }
        public CozeAgent(string token,string botid,string userid) {
            if(!string.IsNullOrEmpty(token))
                TOKEN = token;
            if(!string.IsNullOrEmpty(botid))
                BOT_ID = botid;
            if (!string.IsNullOrEmpty(userid))
                USER_ID = userid;
        }

        public async Task Start() {
            _conversationService = new ConversationService(WEB_BASE_URL, TOKEN);
            var conversation = await _conversationService.CreateAsync();
            if (conversation != null)
            {
                ConversionId = conversation.data.id;
                Console.WriteLine("创建会话成功 - " + ConversionId);
                _webSocketService = new WebSocketService(WEB_SOCKET_URL, TOKEN, BOT_ID, ConversionId, USER_ID);
                _webSocketService.OnMessageEvent += _webSocketService_OnMessageEvent;
                _webSocketService.OnAudioEvent += _webSocketService_OnAudioEvent;

                _audioService = new AudioService();
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        if (_audioService == null)
                            return;
                        byte[]? opusData;
                        if (_audioService.OpusRecordEnqueue(out opusData))
                        {
                            string delta = Convert.ToBase64String(_audioService.ConvertOpusToPcm(opusData, 16000, 1));
                            await _webSocketService.SendAudioAsync(WebSocketProtocol.Input_Audio_Buffer_Append(delta));
                        }

                        await Task.Delay(60);
                    }
                });
            }
            else
            {
                Console.WriteLine("启动失败 - 获取会话失败");
            }

        }

        /// <summary>
        /// 收到语音消息
        /// </summary>
        /// <param name="opus"></param>
        private void _webSocketService_OnAudioEvent(byte[] opus)
        {
            if (_audioService != null)
            {
                _audioService.OpusPlayEnqueue(opus);
            }

            if (OnAudioEvent != null)
                OnAudioEvent(opus);
        }

        /// <summary>
        /// 收到文本消息
        /// </summary>
        /// <param name="message"></param>
        private void _webSocketService_OnMessageEvent(string message)
        {
            if(OnMessageEvent!=null)
                OnMessageEvent(message);
        }

        /// <summary>
        /// 发文本消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message) {
            if(_webSocketService!=null)
                await _webSocketService.SendMessageAsync(WebSocketProtocol.Conversation_Message_Create(message));
        }

        /// <summary>
        /// 开始录音
        /// </summary>
        public void StartRecording()
        {
            //Console.WriteLine("开始录音...");
            if (_audioService != null)
                _audioService.StartRecording();
        }

        /// <summary>
        /// 结束录音
        /// </summary>
        public void StopRecording()
        {
            //Console.WriteLine("结束录音");
            if (_audioService != null && _webSocketService != null)
            {
                _audioService.StopRecording();
                _ = _webSocketService.SendMessageAsync(WebSocketProtocol.Input_Audio_Buffer_Complete());
            }
        }
    }
}
