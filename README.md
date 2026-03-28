# Obsv.Avalonia

一个基于 Avalonia UI 的跨平台桌面应用程序。

## 项目简介

Obsv.Avalonia 是一个使用 .NET 10 和 Avalonia UI 12 构建的现代化桌面应用程序，支持跨平台运行（Windows、Linux、macOS）。

## 主要功能

- 🎨 基于 Fluent 设计主题的现代化 UI
- 📝 集成 AvaloniaEdit 文本编辑器，支持语法高亮
- 🌓 支持亮色/暗色主题切换
- 📁 文件树导航和管理
- 🔧 模块化架构设计（Models、Views、ViewModels、Services）

## 技术栈

- **.NET 10** - 最新的 .NET 框架
- **Avalonia UI 12** - 跨平台 UI 框架
- **AvaloniaEdit** - 强大的文本编辑器组件
- **Newtonsoft.Json** - JSON 序列化/反序列化
- **MVVM 模式** - 清晰的架构分离

## 项目结构

```
Obsv.Avalonia/
├── src/
│   ├── Obsv.Avalonia/              # 主应用程序项目
│   ├── Obsv.Avalonia.Models/       # 数据模型
│   ├── Obsv.Avalonia.ViewModels/   # 视图模型
│   ├── Obsv.Avalonia.Views/        # 视图层
│   └── Obsv.Avalonia.Services/     # 服务层
└── Obsv.Avalonia.slnx              # 解决方案文件
```

## 构建和运行

### 前置要求

- .NET 10 SDK
- 支持 Avalonia UI 的 IDE（推荐 Visual Studio 2022 或 JetBrains Rider）

### 构建项目

```bash
dotnet restore
dotnet build
```

### 运行应用程序

```bash
dotnet run --project src/Obsv.Avalonia/Obsv.Avalonia.csproj
```

### 发布应用程序

```bash
# Windows
dotnet publish -c Release -r win-x64

# Linux
dotnet publish -c Release -r linux-x64

# macOS
dotnet publish -c Release -r osx-x64
```

## 开发指南

### 添加新的视图

1. 在 `Obsv.Avalonia.Views` 中创建新的 `.axaml` 视图文件
2. 在 `Obsv.Avalonia.ViewModels` 中创建对应的 ViewModel
3. 在 `MainWindow.axaml` 中注册新视图

### 添加新的服务

1. 在 `Obsv.Avalonia.Services` 中定义服务接口
2. 实现服务类
3. 在 `App.axaml.cs` 中注册服务

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 贡献

欢迎贡献代码！请遵循以下步骤：

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

## 联系方式

如有问题或建议，请通过以下方式联系：

- 提交 Issue
- 发送邮件至项目维护者
