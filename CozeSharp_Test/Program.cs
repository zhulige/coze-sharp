using CozeSharp;

class Program
{
    private static bool _status = false;
    private static CozeAgent? _cozeAgent = null;
    static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Title = "扣子AI 调试助手";
        string logoAndCopyright = @"
========================================================================
欢迎使用“扣子AI 服务器调试控制台” ！版本 v1.0.1

当前功能：
1. 语音消息 输入回车：开始录音；再次输入回车：接收录音
2. 文字消息 可以随意输入文字对话
3. 全量往返协议输出，方便调试
        
要是你在使用中有啥想法或者遇到问题，别犹豫，找我们哟：
微信：Zhu-Lige       电子邮箱：ZhuLige@qq.com
有任何动态请大家关注 https://github.com/zhulige/coze-sharp
========================================================================";
        Console.WriteLine(logoAndCopyright);
        _cozeAgent = new CozeAgent();
        _cozeAgent.Start();

        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                if (_status == false)
                {
                    _status = true;
                    _cozeAgent.StartRecording();
                    Console.Title = "扣子AI 开始录音...";
                    Console.WriteLine("开始录音... 再次回车结束录音");
                    continue;
                }
                else
                {
                    if (_status == true)
                    {
                        _status = false;
                        _cozeAgent.StopRecording();
                        Console.Title = "扣子AI 调试助手";
                        Console.WriteLine("结束录音");
                        continue;
                    }
                }
                continue;
            }
            else
            {
                if (_status == false)
                    await _cozeAgent.SendMessageAsync(input);
            }

        }
    }
}