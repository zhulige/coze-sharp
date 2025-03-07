using CozeSharp;

class Program
{
    private static bool _status = false;
    private static CozeAgent? _cozeAgent = null;
    static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Title = "扣子AI 调试助手";

        _cozeAgent = new CozeAgent();

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