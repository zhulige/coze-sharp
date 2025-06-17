# 扣子Coze Chat SDK

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

## 项目简介 
CozeSharp 是使用 C# 语言编写的 “扣子Coze Chat SDK”，并提供了ConsoleApp 应用。

**跨平台支持**：本项目支持以下平台：
- **操作系统**：Windows、MacOS、Linux、Android、IOS
- **硬件平台**：x86、x86_64、arm、arm_64
- **开发板**：ASUS Tinker Board2s、Raspberry Pi

## 示例
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

详见 CozeSharp_ConsoleApp 项目。

## NuGet
```
dotnet add package CozeSharp --version 1.0.2
```

## 相关资源
https://opus-codec.org/downloads/

## 贡献与反馈
如果你在使用过程中发现了项目中的问题，或者有任何改进的建议，欢迎随时提交 Issue 或者 Pull Request。你的反馈和贡献将对项目的发展和完善起到重要的作用。

## 加入社群
欢迎加入我们的社区，分享经验、提出建议或获取帮助！

## 项目统计
[![Star History Chart](https://api.star-history.com/svg?repos=zhulige/coze-sharp&type=Date)](https://www.star-history.com/#zhulige/coze-sharp&Date)
