using CozeSharp.Protocols;
using CozeSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CozeSharp.Serivces
{
    public class WebSocketService
    {
        public delegate void MessageEventHandler(string message);
        public delegate void AudioEventHandler(byte[] opus);
        public event MessageEventHandler? OnMessageEvent = null;
        public event AudioEventHandler? OnAudioEvent = null;

        private string? _webSocketUrl { get; set; }
        private string? _token { get; set; }
        private string? _botId { get; set; }
        private string? _conversionId { get; set; }
        private string? _userId { get; set; }

        private ClientWebSocket? _webSocketChat = null;

        public WebSocketService(string wsUrlChat, string token, string botId, string conversionId, string userId)
        {
            if (!string.IsNullOrEmpty(token))
                _token = token;
            if (!string.IsNullOrEmpty(wsUrlChat))
                _webSocketUrl = wsUrlChat;
            if (!string.IsNullOrEmpty(botId))
                _botId = botId;
            if (!string.IsNullOrEmpty(conversionId))
                _conversionId = conversionId;
            if (!string.IsNullOrEmpty(userId))
                _userId = userId;

            Uri uriChat = new Uri(_webSocketUrl + "/v1/chat?bot_id=" + _botId);
            _webSocketChat = new ClientWebSocket();
            _webSocketChat.Options.SetRequestHeader("Authorization", "Bearer " + _token);
            _webSocketChat.ConnectAsync(uriChat, CancellationToken.None);
            
            LogConsole.WriteLine(uriChat.ToString());
            LogConsole.WriteLine("WebSocket Chat 初始化完成");

            LogConsole.Write("WebSocket Chat 连接中...");
            while (_webSocketChat.State != WebSocketState.Open)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.WriteLine("");
            LogConsole.WriteLine("WebSocket 连接成功 WebSocket.State:" + _webSocketChat.State.ToString());
            
            // WebSocket 接收消息
            Task.Run(async () =>
            {
                await ReceiveMessagesAsync();
            });

            // WebSocket 重连
            Task.Run(async () =>
            {
                if (_webSocketChat.State != WebSocketState.Open)
                {
                    await _webSocketChat.ConnectAsync(uriChat, CancellationToken.None);
                }
                await Task.Delay(1000);
            });
        }

        /// <summary>
        /// 接收WebSocket消息
        /// </summary>
        /// <returns></returns>
        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024];
            try
            {
                if (_webSocketChat == null)
                    return;

                await SendMessageAsync(WebSocketProtocol.Chat_Update(_conversionId, _userId));

                while (_webSocketChat.State == WebSocketState.Open)
                {
                    var result = await _webSocketChat.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        if (!string.IsNullOrEmpty(message))
                        {
                            LogConsole.ReceiveLine($"WebSocket 接收到消息: {message}");
                            dynamic? msg = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(message);
                            if (msg != null)
                            {
                                //if (msg.event_type == "conversation.message.delta") { //文字流
                                if (msg.event_type == "conversation.message.completed")
                                {
                                    //Console.WriteLine($"WebSocket 接收到消息: {msg.event_type}");
                                    if (msg.data.content_type == "text")
                                    {
                                        LogConsole.WriteLine($"WebSocket 接收到消息: {msg.data.role} ： {msg.data.content}");
                                        if (msg.data.role == "assistant")
                                        {
                                            if (OnMessageEvent != null)
                                                OnMessageEvent((string)msg.data.content);
                                        }
                                    }
                                }

                                if (msg.event_type == "conversation.audio.delta")
                                {
                                    if (msg.data.content_type == "audio")
                                    {
                                        LogConsole.ReceiveLine($"WebSocket 接收到语音: {msg.data.content}");
                                        byte[] opusBytes = Convert.FromBase64String((string)msg.data.content);
                                        if (OnAudioEvent != null)
                                        {
                                            OnAudioEvent(opusBytes);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        //LogConsole.WriteLine($"WebSocket 接收到语音: {buffer.Length}");
                    }
                    await Task.Delay(10);
                }
            }
            catch (Exception ex)
            {
                LogConsole.ErrorLine($"WebSocket 接收消息时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送WebSocket消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message)
        {
            if (_webSocketChat == null)
                return;

            if (_webSocketChat.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocketChat.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                LogConsole.SendLine($"WebSocket 发送的消息: {message}");
            }
        }

        /// <summary>
        /// 发送WebSocket语音消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAudioAsync(string message)
        {
            if (_webSocketChat == null)
                return;

            if (_webSocketChat.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocketChat.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                LogConsole.SendLine($"WebSocket 发送的语音: {message}");
            }
        }

        /// <summary>
        /// 发送WebSocket语音包Opus
        /// </summary>
        /// <param name="opus"></param>
        /// <returns></returns>
        public async Task SendOpusAsync(byte[] opus)
        {
            if (_webSocketChat == null)
                return;

            if (_webSocketChat.State == WebSocketState.Open)
            {

                //await _webSocketChat.SendAsync(new ArraySegment<byte>(opus), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }
}
