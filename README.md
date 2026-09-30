# STS2 Mod Rewrite

《杀戮尖塔2》模组重写项目。

## 技术栈
- C# / .NET 9.0
- Godot 4.5.1
- STS2 Modding API

## 目录结构
- `src/Core/` — 日志、配置、事件总线、工具类
- `src/Adapters/` — 外部知识库适配层
- `src/Cards/` — 卡牌
- `src/Relics/` — 遗物
- `src/Powers/` — 能力
- `src/Events/` — 事件
- `src/UI/` — 界面
- `src/Utils/` — 通用工具

## 文档
- `CONVENTIONS.md` — 命名与编码规范
- `AGENTS.md` — AI使用说明
- `ARCHITECTURE.md` — 架构说明
- `API_NOTES.md` — 接口速查
- `CONTRIBUTING.md` — 协作流程

## 构建

```
dotnet build -c Release
```

## 分支
- `main` — 稳定版
- `develop` — 开发版