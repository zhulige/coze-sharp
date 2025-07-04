# Coze Chat SDK
<p>
  <a href="https://github.com/zhulige/coze-sharp/releases/latest">
    <img src="https://img.shields.io/github/v/release/zhulige/coze-sharp?style=flat-square&logo=github&color=blue" alt="Release"/>
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-green.svg?style=flat-square" alt="License: MIT"/>
  </a>
  <a href="https://github.com/zhulige/coze-sharp/stargazers">
    <img src="https://img.shields.io/github/stars/zhulige/coze-sharp?style=flat-square&logo=github" alt="Stars"/>
  </a>
  <a href="https://github.com/zhulige/coze-sharp/releases/latest">
    <img src="https://img.shields.io/github/downloads/zhulige/coze-sharp/total?style=flat-square&logo=github&color=52c41a1&maxAge=86400" alt="Download"/>
  </a>
</p>

## Project Introduction
CozeSharp is the "Coze Chat SDK" developed in C# language, accompanied by a ConsoleApp demonstration.

## Example
``` C#
using CozeSharp;

CozeAgent agent = new CozeAgent();
agent.Token = configuration["CozeSettings:Token"];
agent.BotId = configuration["CozeSettings:BotId"];
agent.UserId = configuration["CozeSettings:UserId"];
agent.OnMessageEvent += Agent_OnMessageEvent;
await agent.Start();

private static Task Agent_OnMessageEvent(string type, string message)
{
    LogConsole.InfoLine($"[{type}] {message}");
    return Task.CompletedTask;
}
```

## NuGet
```
dotnet add package CozeSharp --version 1.0.1
```

## Related Resources
https://opus-codec.org/downloads/

## Contributions & Feedback
If you encounter issues during use or have suggestions for improvement, feel free to submit Issues or Pull Requests. Your feedback and contributions are crucial for the project's growth and refinement.

## Join Our Community
We welcome you to join our community to share experiences, offer suggestions, or seek assistance!
