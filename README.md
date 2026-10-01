# STS2 Mod

《杀戮尖塔2》模组，基于 RitsuLib。

## 技术栈
- C# / .NET 9.0
- Godot 4.5.1（Mono 版）
- STS2 Modding API
- RitsuLib

## 目录结构
- `Sts2Mod/` — 资源目录（图片、本地化、场景）
  - `images/` — 图片
  - `localization/` — 本地化文本
  - `scenes/` — Godot 场景
- `Sts2ModCode/` — C# 代码
  - `Cards/` — 卡牌
  - `Characters/` — 角色、卡池、遗物池、药水池
  - `Relics/` — 遗物
  - `Entry.cs` — 入口

## 文档
- `CONVENTIONS.md` — 命名与编码规范
- `AGENTS.md` — AI 使用说明
- `ARCHITECTURE.md` — 架构说明
- `API_NOTES.md` — 接口速查
- `CONTRIBUTING.md` — 协作流程

## 构建

先复制 `local.props.template` 为 `local.props`，填入本机路径。

```
dotnet build
```

仅编译 C#（不导出 PCK）：

```
dotnet build /p:RunPckExport=false
```

## 参考
- [RitsuLib 仓库](https://github.com/BAKAOLC/STS2-RitsuLib)
- [RitsuLib 文档](https://github.com/GlitchedReme/SlayTheSpire2ModdingTutorials/tree/master/RitsuLib)