using CozeSharp;
using CozeSharp.Utils;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;

class Program
{
    private static bool _recordStatus = false;
    static async Task Main(string[] args)
    {
        // 创建配置构建器
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

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
                    Console.Title = "停止录音";
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