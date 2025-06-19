using CozeSharp;
using CozeSharp.Utils;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;

class Program
{
    private static bool _recordStatus = false;
    static async Task Main(string[] args)
    {
        Console.Title = "扣子CozeSharp客户端";
        string logoAndCopyright = @"
========================================================================
欢迎使用“扣子CozeSharp客户端” ！ 

当前功能：
1. 语音消息 输入回车：开始录音；再次输入回车：结束录音
2. 文字消息 可以随意输入文字对话
3. 全量往返协议输出，方便调试
        
要是你在使用中有啥想法或者遇到问题，别犹豫，找我们哟：
微信：Zhu-Lige       电子邮箱：zhuLige@qq.com
有任何动态请大家关注 https://github.com/zhulige/coze-sharp
========================================================================";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(logoAndCopyright);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("使用前请配置 appsettings.json ！");
        Console.WriteLine("========================================================================");
        Console.ForegroundColor = ConsoleColor.White;

        // 创建配置构建器
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        CozeSharp.Global.IsDebug = true;
        CozeAgent _agent = new CozeAgent();
        _agent.Token = configuration["CozeSettings:Token"];
        _agent.BotId = configuration["CozeSettings:BotId"];
        _agent.UserId = configuration["CozeSettings:UserId"];
        _agent.OnMessageEvent += Agent_OnMessageEvent;
        //agent.OnAudioEvent += Agent_OnAudioEvent;
        await _agent.Start();

        _ = Task.Run(async () =>
        {
            while (true)
            {
                bool isCapsLockOn = Console.CapsLock;
                //Console.WriteLine($"当前 Caps Lock 状态: {(isCapsLockOn ? "开启" : "关闭")}");
                if (isCapsLockOn)
                {
                    if (_recordStatus == false)
                    {
                        _recordStatus = true;
                        LogConsole.InfoLine("开始录音... 再次按Caps键结束录音");
                        await _agent.StartRecording();
                        continue;
                    }
                }
                if (!isCapsLockOn)
                {
                    if (_recordStatus == true)
                    {
                        _recordStatus = false;
                        await _agent.StopRecording();
                        LogConsole.InfoLine("结束录音");
                        continue;
                    }
                }
                await Task.Delay(100); // 避免过于频繁的检查
            }
        });

        while (true)
        {
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                await _agent.ChatMessage(input);
            }
            else
            {
                //if (!_recordStatus)
                //{
                //    _recordStatus = true;
                //    Console.Title = "开始录音...";
                //    Console.WriteLine("开始录音... 再次回车结束录音");
                //    await agent.StartRecording();
                //}
                //else
                //{
                //    await agent.StopRecording();
                //    Console.Title = "扣子CozeSharp客户端";
                //    Console.WriteLine("结束录音");
                //    _recordStatus = false;
                //}
            }
        }
    }

    private static Task Agent_OnMessageEvent(string type, string message)
    {
        switch (type.ToLower())
        {
            case "question":
                LogConsole.WriteLine(MessageType.Send, $"[{type}] {message}");
                break;
            case "answer":
                LogConsole.WriteLine(MessageType.Recv, $"[{type}] {message}");
                break;
            default:
                LogConsole.InfoLine($"[{type}] {message}");
                break;
        }
        //LogConsole.InfoLine($"[{type}] {message}");
        return Task.CompletedTask;
    }

    private static Task Agent_OnAudioEvent(byte[] opus)
    {
        throw new NotImplementedException();
    }
}