using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozeSharp.Protocols
{
    public class WebSocketProtocol
    {
        public WebSocketProtocol() { }

        public static string Chat_Update(string conversation_id,string user_id)
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
                        ""parameters"": {""custom_var_1"": ""测试""}
                    },
                    ""input_audio"": {         
                        ""format"": ""pcm"",   
                        ""codec"": ""pcm"",
                        ""samplate_rate"": 16000,
                        ""channel"": 1, 
                        ""bit_depth"": 16 
                    },
                    ""output_audio"": {   
                        ""codec"": ""opus"", 
                        ""opus_config"": {
                            ""bitrate"": 16000,
                            ""use_cbr"": false,
                            ""frame_size_ms"": 60,
                            ""limit_config"": {
                                ""period"": 1, 
                                ""max_frame_num"": 300 
                            }
                        },
                        ""speech_rate"": 50, 
                        ""voice_id"": ""7426720361732915209""
                    }
                }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Conversation_Id>", conversation_id).Replace("<User_Id>", user_id);
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }

        public static string Input_Audio_Buffer_Append(string delta) {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_audio_buffer.append"",
              ""data"": {
                 ""delta"": ""<Delta>""
              }
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            message = message.Replace("<Delta>", delta);
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }

        public static string Input_Audio_Buffer_Complete()
        {
            string message = @"{
              ""id"": ""<Event_Id>"",
              ""event_type"": ""input_audio_buffer.complete""
            }";
            message = message.Replace("\n", "").Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
            message = message.Replace("<Event_Id>", Guid.NewGuid().ToString());
            //Console.WriteLine($"发送的消息: {message}");
            return message;
        }

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

    }
}
