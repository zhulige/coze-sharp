using System;

namespace CozeSharp.Protocols
{
    public class Coze_Protocol
    {
        #region 双向流式语音合成
        /// <summary>
        /// 更新语音合成配置
        /// </summary>
        public static string Speech_Update(string voice_id = "7426720361753968677")
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""speech.update"",
              ""data"": {
                  ""output_audio"": {   
                        ""codec"": ""opus"", 
                        ""opus_config"": {
                            ""bitrate"": 24000,
                            ""use_cbr"": false,
                            ""frame_size_ms"": 60,
                            ""limit_config"": {
                                ""period"": 1, 
                                ""max_frame_num"": 300 
                            }
                        },
                        ""speech_rate"": 0, 
                        ""voice_id"": ""<Voice_Id>""
                    }
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Voice_Id>", voice_id);
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }

        /// <summary>
        /// 流式输入文字
        /// </summary>
        public static string Input_Text_Buffer_Append(string text)
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_text_buffer.append"",
              ""data"": {
                  ""delta"": ""<Text>""
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Text>", text);
            return message;
        }

        /// <summary>
        /// 提交文字
        /// </summary>
        /// <returns></returns>
        public static string Input_Text_Buffer_Complete()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_text_buffer.complete""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            return message;
        }
        #endregion

        #region 双向流式语音识别
        /// <summary>
        /// 更新语音识别配置
        /// </summary>
        public static string Transcriptions_Update()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""transcriptions.update"",
              ""data"": {
                  ""input_audio"": {
                      ""format"": ""pcm"",
                      ""codec"": ""pcm"",
                      ""sample_rate"": 24000,
                      ""channel"": 1,
                      ""bit_depth"": 16
                  }
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            return message;
        }
        /// <summary>
        /// 流式上传音频片段
        /// </summary>
        public static string Input_Audio_Buffer_Append(string base64)
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_audio_buffer.append"",
              ""data"": {
                 ""delta"": ""<EncodedAudioDelta>""
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<EncodedAudioDelta>", base64);
            return message;
        }
        /// <summary>
        /// 提交音频
        /// </summary>
        public static string Input_Audio_Buffer_Complete()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_audio_buffer.complete""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            return message;
        }
        #endregion

        #region 双向流式对话
        /// <summary>
        /// 更新对话配置
        /// </summary>
        public static string Chat_Update(string conversation_id, string user_id, string voice_id = "7426720361753968677",string user_language="zh")
        {
            string message = @"{
                ""id"": ""<Event_Id>"",
                ""event_type"": ""chat.update"",
                ""data"": {
                    ""chat_config"": {
                        ""auto_save_history"": true, 
                        ""conversation_id"": ""<Conversation_Id>"", 
                        ""user_id"": ""<User_Id>"",  
                        ""meta_data"": {}, 
                        ""custom_variables"": {}, 
                        ""extra_params"": {},   
                        ""parameters"": {}
                    },
                    ""input_audio"": {         
                        ""format"": ""pcm"",   
                        ""codec"": ""pcm"",
                        ""samplate_rate"": 24000,
                        ""channel"": 1, 
                        ""bit_depth"": 16 
                    },
                    ""output_audio"": {   
                        ""codec"": ""opus"", 
                        ""opus_config"": {
                            ""bitrate"": 24000,
                            ""use_cbr"": false,
                            ""frame_size_ms"": 60,
                            ""limit_config"": {
                                ""period"": 1, 
                                ""max_frame_num"": 18
                            }
                        },
                        ""speech_rate"": 0, 
                        ""voice_id"": ""<Voice_Id>""
                    },
                    ""asr_config"":{
                        ""user_language"": ""<User_Language>""
                    }
                }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Conversation_Id>", conversation_id).Replace("<User_Id>", user_id);
            message = message.Replace("<Voice_Id>", voice_id);
            message = message.Replace("<User_Language>", user_language);
            return message;
        }
        /// <summary>
        /// 清除缓冲区音频
        /// </summary>
        public static string Input_Audio_Buffer_Clear()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_audio_buffer.clear""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }
        /// <summary>
        /// 手动提交对话内容
        /// </summary>
        /// <param name="text"></param>
        public static string Conversation_Message_Create(string text)
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""conversation.message.create"",
                ""data"": {
                     ""role"": ""user"", 
                     ""content_type"": ""text"", 
                     ""content"": ""[{\""type\"":\""text\"",\""text\"":\""<Text>\""}]""
                  }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Text>", text);
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }

        /// <summary>
        /// 手动提交对话内容
        /// </summary>
        /// <param name="text"></param>
        public static string Conversation_Message_Create(string text,string url)
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""conversation.message.create"",
                ""data"": {
                     ""role"": ""user"", 
                     ""content_type"": ""object_string"", 
                     ""content"": ""[
                            {\""text\"":\""<Text>\"",\""url\"": \""<Url>\""}
                        ]""
                  }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "").Replace("&nbsp;"," ");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Text>", text);
            message = message.Replace("<Url>", url);
            return message;
        }
        /// <summary>
        /// 打断智能体输出
        /// </summary>
        public static string Conversation_Chat_Cancel()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""conversation.chat.cancel""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            return message;
        }
        /// <summary>
        /// 清除上下文
        /// </summary>
        public static string Conversation_Clear()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""conversation.clear""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            return message;
        }
        /// <summary>
        /// 提交端插件执行结果
        /// </summary>
        /// <returns></returns>
        public static string Conversation_Chat_Submit_Tool_Outputs(string chatId, string tool_call_id, string output)
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""conversation.chat.submit_tool_outputs"",
              ""data"": {
                  ""chat_id"": ""<Chat_Id>"",
                  ""tool_outputs"": [
                      {
                          ""tool_call_id"": ""<Tool_Call_Id>"",
                          ""output"": ""<Output>""
                      }
                  ]
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Chat_Id>", chatId);
            message = message.Replace("<Tool_Call_Id>", tool_call_id);
            message = message.Replace("<Output>", output);
            return message;
        }
        #endregion
    }
}
