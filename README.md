# STS2 Mod

《杀戮尖塔2》模组，基于 RitsuLib。

## 技术栈

- C# 13 / .NET 9.0
- Godot 4.5.1（Mono 版）
- STS2 游戏 API 0.111.0
- RitsuLib 0.6.3

## 目录结构

- `Molin/` — 资源目录（图片、本地化、场景）
  - `images/` — 图片
  - `localization/` — 本地化文本
  - `scenes/` — Godot 场景
- `MolinCode/` — C# 代码
  - `Cards/` — 卡牌
  - `Characters/` — 角色、卡池、遗物池、药水池
  - `Relics/` — 遗物
- `MolinCode/Entry.cs` — 入口（Mod 初始化、`ResPath`、`Logger`）

## 文档

- `CONVENTIONS.md` — 命名与编码规范
- `AGENTS.md` — AI 使用说明
- `ARCHITECTURE.md` — 架构说明
- `API_NOTES.md` — 接口速查
- `CONTRIBUTING.md` — 协作流程
- `docs/features.md` — 功能清单
- `docs/assets.md` — 资源清单
- `docs/decisions.md` — 决策记录
- `docs/regression.md` — 回归测试记录

## 构建

### 前置

1. 复制 `local.props.template` 为 `local.props`
2. 填入 `Sts2Dir`（游戏根目录）和 `Sts2DataDir`（DLL 所在目录）
3. 填入 `RitsuLibDir`（本机 RitsuLib 安装目录，需含 `RitsuLib.References.props`、`compat/`、`shared/`）
4. 如需导出 PCK，填入 `GodotExe`

RitsuLib 程序集引用由 `Molin.csproj` 里的 `RitsuLibReferenceTarget`（当前 `0.111.0`）加上各机器 `local.props` 的 `RitsuLibDir` 决定：`Molin.csproj` 里不写任何本机路径，两台机器各自指向本机的 RitsuLib 安装目录，安装目录自带的 `RitsuLib.References.props` 会被自动导入。

两台机器必须使用**同一游戏 API 版本**的 `sts2.dll`（当前 `0.111.0`）和**同一份** RitsuLib（工坊变体包 `0.6.3`）。`MolinStrike` 的 `FromCard(卡牌, CardPlay)` 签名来自 `0.111.0` 的游戏程序集，版本不一致会让其中一台编不过。

### 命令

```powershell
# 只编译、不导出 PCK（仍会拷贝到游戏 mods 目录）
dotnet build /p:RunPckExport=false

# 完整构建（含 PCK 导出，需要 GodotExe）
dotnet build
```

## 参考

- [本仓库](https://github.com/hwjsldgj/sts2-mod)
- [RitsuLib 仓库](https://github.com/BAKAOLC/STS2-RitsuLib)
- [RitsuLib 文档](https://github.com/GlitchedReme/SlayTheSpire2ModdingTutorials/tree/master/RitsuLib)

## 作者

- [YGG-sudo](https://github.com/YGG-sudo)
- [hwjsldgj](https://github.com/hwjsldgj)
