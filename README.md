# Obsv

一个轻量级的文本和代码阅读器，基于 Avalonia UI 构建的跨平台桌面应用程序。

A lightweight text and code reader built with Avalonia UI for cross-platform desktop applications.

## 项目简介 / Project Overview

Obsv 是一个使用 .NET 10 和 Avalonia UI 12 构建的轻量级文本和代码阅读器，支持跨平台运行（Windows、Linux、macOS）。专注于提供简洁、高效的文本阅读和代码编辑体验。

Obsv is a lightweight text and code reader built with .NET 10 and Avalonia UI 12, supporting cross-platform operation (Windows, Linux, macOS). It focuses on providing a simple and efficient text reading and code editing experience.

## 主要功能 / Features

- 📖 轻量级文本阅读和编辑 / Lightweight text reading and editing
- 🎨 基于 Fluent 设计主题的现代化 UI / Modern UI based on Fluent design theme
- 📝 集成 AvaloniaEdit 文本编辑器，支持语法高亮 / Integrated AvaloniaEdit text editor with syntax highlighting
- 🌓 支持亮色/暗色主题切换 / Light/Dark theme switching support
- 📁 文件树导航和管理 / File tree navigation and management
- 🔧 模块化架构设计（Models、Views、ViewModels、Services）/ Modular architecture design (Models, Views,ViewModels, Services)

## 技术栈 / Tech Stack

- **.NET 10** - 最新的 .NET 框架 / Latest .NET framework
- **Avalonia UI 12** - 跨平台 UI 框架 / Cross-platform UI framework
- **AvaloniaEdit** - 强大的文本编辑器组件 / Powerful text editor component
- **Newtonsoft.Json** - JSON 序列化/反序列化 / JSON serialization/deserialization
- **MVVM 模式** - 清晰的架构分离 / Clear architecture separation

## 项目结构 / Project Structure

```
Obsv/
├── src/
│   ├── Obsv.Avalonia/              # 主应用程序项目 / Main application project
│   ├── Obsv.Avalonia.Models/       # 数据模型 / Data models
│   ├── Obsv.Avalonia.ViewModels/   # 视图模型 / View models
│   ├── Obsv.Avalonia.Views/        # 视图层 / Views layer
│   └── Obsv.Avalonia.Services/     # 服务层 / Services layer
└── Obsv.Avalonia.slnx              # 解决方案文件 / Solution file
```

## 构建和运行 / Build and Run

### 前置要求 / Prerequisites

- .NET 10 SDK
- 支持 Avalonia UI 的 IDE（推荐 Visual Studio 2022 或 JetBrains Rider）/ IDE with Avalonia UI support (Visual Studio 2022 or JetBrains Rider recommended)

### 构建项目 / Build the Project

```bash
dotnet restore
dotnet build
```

### 运行应用程序 / Run the Application

```bash
dotnet run --project src/Obsv.Avalonia/Obsv.Avalonia.csproj
```

### 发布应用程序 / Publish the Application

```bash
# Windows (Obsv)
dotnet publish -c Release -r win-x64

# Linux (Obsv)
dotnet publish -c Release -r linux-x64

# macOS (Obsv)
dotnet publish -c Release -r osx-x64
```

## 开发指南 / Development Guide

### 添加新的视图 / Adding New Views

1. 在 `Obsv.Avalonia.Views` 中创建新的 `.axaml` 视图文件 / Create a new `.axaml` view file in `Obsv.Avalonia.Views`
2. 在 `Obsv.Avalonia.ViewModels` 中创建对应的 ViewModel / Create the corresponding ViewModel in `Obsv.Avalonia.ViewModels`
3. 在 `MainWindow.axaml` 中注册新视图 / Register the new view in `MainWindow.axaml`

### 添加新的服务 / Adding New Services

1. 在 `Obsv.Avalonia.Services` 中定义服务接口 / Define the service interface in `Obsv.Avalonia.Services`
2. 实现服务类 / Implement the service class
3. 在 `App.axaml.cs` 中注册服务 / Register the service in `App.axaml.cs`

## 许可证 / License

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 贡献 / Contributing

欢迎贡献代码！请遵循以下步骤：

Contributions are welcome! Please follow these steps:

1. Fork 本仓库 / Fork the repository
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`) / Create a feature branch
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`) / Commit your changes
4. 推送到分支 (`git push origin feature/AmazingFeature`) / Push to the branch
5. 开启 Pull Request / Open a Pull Request

## 联系方式 / Contact

如有问题或建议，请通过以下方式联系：

For questions or suggestions, please contact us through:

- 提交 Issue / Submit an Issue
- 发送邮件至项目维护者 / Send an email to the project maintainers
