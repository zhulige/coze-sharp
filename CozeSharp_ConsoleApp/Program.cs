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

        CozeSharp.Global.IsDebug = false;
        CozeAgent agent = new CozeAgent();
        agent.Token = configuration["CozeSettings:Token"];
        agent.BotId = configuration["CozeSettings:BotId"];
        agent.UserId = configuration["CozeSettings:UserId"];
        agent.OnMessageEvent += Agent_OnMessageEvent;
        //agent.OnAudioEvent += Agent_OnAudioEvent;
        await agent.Start();

        while (true)
        {
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                await agent.ChatMessage(input);
            }
            else
            {
                if (!_recordStatus)
                {
                    _recordStatus = true;
                    Console.Title = "开始录音...";
                    Console.WriteLine("开始录音... 再次回车结束录音");
                    await agent.StartRecording();
                }
                else
                {
                    await agent.StopRecording();
                    Console.Title = "扣子CozeSharp客户端";
                    Console.WriteLine("结束录音");
                    _recordStatus = false;
                }
            }
        }
    }

    private static Task Agent_OnMessageEvent(string type, string message)
    {
        LogConsole.InfoLine($"[{type}] {message}");
        return Task.CompletedTask;
    }

    private static Task Agent_OnAudioEvent(byte[] opus)
    {
        throw new NotImplementedException();
    }
}