using CozeSharp.Protocols;
using CozeSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CozeSharp.Serivces.Chat
{
    public class ChatService
    {
        private string TAG = "扣子";
        private string _wsUrl { get; set; } = "wss://ws.coze.cn";
        private string _token { get; set; } = string.Empty;
        private string _botId { get; set; } = string.Empty;
        // 首次连接
        private bool _isFirst = true;

        private ClientWebSocket? _webSocket = null;

        #region 属性
        public string ConversionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string VoiceId { get; set; } = "7468518753626669093";
        public string UserLanguage { get; set; } = "zh";
        #endregion

        #region 事件
        public delegate Task MessageEventHandler(string type, string message);
        public event MessageEventHandler? OnMessageEvent = null;

        public delegate Task AudioEventHandler(byte[] opus);
        public event AudioEventHandler? OnAudioEvent = null;
        #endregion

        #region 构造函数
        public ChatService(string wsUrl, string token, string botId)
        {
            _wsUrl = wsUrl;
            _token = token;
            _botId = botId;
        }
        #endregion

        public void Start()
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(_botId))
            {
                LogConsole.ErrorLine($"{TAG} 启动失败 - 令牌或智能体Id不能为空！");
                return;
            }
            Uri uri = new Uri(_wsUrl + "/v1/chat?bot_id=" + _botId);
            _webSocket = new ClientWebSocket();
            _webSocket.Options.SetRequestHeader("Authorization", "Bearer " + _token);
            _webSocket.ConnectAsync(uri, CancellationToken.None);
            LogConsole.InfoLine($"{TAG} 连接中...");

            Task.Run(async () =>
            {
                await ReceiveMessagesAsync();
            });
        }

        private async Task ReceiveMessagesAsync()
        {
            if (_webSocket == null)
                return;

            var buffer = new byte[1024 * 10];
            while (true)
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        // 首次
                        if (_isFirst)
                        {
                            _isFirst = false;
                            LogConsole.InfoLine($"{TAG} 连接成功");
                            await SendMessageAsync(Coze_Protocol.Chat_Update(ConversionId, UserId, VoiceId, UserLanguage));
                        }

                        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        byte[] messageBytes = new byte[result.Count];
                        Array.Copy(buffer, messageBytes, result.Count);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var message = Encoding.UTF8.GetString(messageBytes);
                            if (Global.IsDebug)
                            {
                                if (message.Contains("conversation.audio.delta")
                                || message.Contains("conversation.message.delta")
                                ) { }
                                else
                                {
                                    LogConsole.ReceiveLine($"{TAG} {message}");
                                }
                            }

                            if (!string.IsNullOrEmpty(message))
                            {
                                dynamic? msg = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(message);
                                if (msg == null)
                                {
                                    LogConsole.ErrorLine($"{TAG} 接收到的消息格式错误: {message}");
                                    continue;
                                }

                                // 错误消息
                                if (msg.event_type == "error")
                                {
                                    if (OnMessageEvent != null)
                                        await OnMessageEvent("error", (string)msg.data.msg);
                                }
                                // 接收消息
                                if (msg.event_type == "conversation.message.delta")
                                {
                                    if (msg.data.type == "answer")
                                    {

                                    }

                                }
                                // 事件完成
                                if (msg.event_type == "conversation.message.completed")
                                {
                                    if (msg.data.type == "question")
                                    {
                                        if (OnMessageEvent != null)
                                            await OnMessageEvent("question", (string)msg.data.content);
                                    }
                                    if (msg.data.type == "answer")
                                    {
                                        if (OnMessageEvent != null)
                                            await OnMessageEvent("answer", (string)msg.data.content);
                                    }
                                    if (msg.data.type == "function_call")
                                    {
                                        if (OnMessageEvent != null)
                                            await OnMessageEvent("function_call", (string)msg.data.content);
                                    }
                                }

                                // 语音识别
                                if (msg.event_type == "conversation.audio_transcript.update")
                                {
                                    
                                }

                                // 接收语音
                                if (msg.event_type == "conversation.audio.delta")
                                {
                                    await Task.Run(async () =>
                                    {
                                        byte[] opusBytes = Convert.FromBase64String((string)msg.data.content);
                                        if (OnAudioEvent != null)
                                            await OnAudioEvent(opusBytes);
                                    });
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        LogConsole.ErrorLine($"{TAG} {ex.Message}");
                        break;
                    }
                }
            }
        }

        private async Task SendMessageAsync(string message)
        {
            if (_webSocket == null)
                return;

            if (_webSocket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                LogConsole.SendLine($"{TAG} {message}");
            }
        }

        /// <summary>
        /// 发送WebSocket语音消息
        /// </summary>
        /// <param name="base64">pcm</param>
        /// <returns></returns>
        public async Task SendAudio_Buffer_AppendAsync(string base64)
        {
            if (_webSocket == null)
                return;

            if (_webSocket.State == WebSocketState.Open)
            {
                string message = Coze_Protocol.Input_Audio_Buffer_Append(base64);
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        /// <summary>
        /// 发送WebSocket语音包pcm
        /// </summary>
        /// <param name="opus"></param>
        /// <returns></returns>
        public async Task SendAudio_PCM_AppendAsync(byte[] pcmBytes)
        {
            if (_webSocket == null)
                return;

            if (_webSocket.State == WebSocketState.Open)
            {
                string delta = Convert.ToBase64String(pcmBytes);
                await SendAudio_Buffer_AppendAsync(delta);
            }
        }

        public async Task SendAudio_Buffer_CompleteAsync()
        {
            await SendMessageAsync(Coze_Protocol.Input_Audio_Buffer_Complete());
        }

        public async Task SendChat_UpdateAsync()
        {
            await SendMessageAsync(Coze_Protocol.Chat_Update(ConversionId, UserId, VoiceId, UserLanguage));
        }

        /// <summary>
        /// 打断消息
        /// </summary>
        /// <returns></returns>
        public async Task ChatAbort()
        {
            await SendMessageAsync(Coze_Protocol.Conversation_Chat_Cancel());
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ChatMessage(string message)
        {
            await SendMessageAsync(Coze_Protocol.Conversation_Message_Create(message));
        }

    }
}
