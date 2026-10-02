# 架构说明

## 项目结构
- `Molin/` — Godot 资源目录，会被打包进 PCK
  - `images/` — 图片
  - `localization/` — 本地化 JSON
  - `scenes/` — Godot 场景（角色立绘、能量表、商店与篝火等）
- `MolinCode/` — C# 源码
  - `Entry.cs` — Mod 入口，`[ModInitializer]`
  - `Cards/` — 卡牌
  - `Characters/` — 角色、卡池、遗物池、药水池
  - `Relics/` — 遗物

## 框架
- 游戏 API：`MegaCrit.Sts2.*`
- 模组框架：`STS2RitsuLib.*`
- 补丁：`0Harmony`
- 资源引擎：Godot 4.5.1

## 内容注册
- 卡牌：`[RegisterCard(typeof(卡池))]`
- 遗物：`[RegisterRelic(typeof(遗物池))]`
- 角色：`[RegisterCharacter]`
- 初始卡/遗物：`[RegisterCharacterStarterCard]` / `[RegisterCharacterStarterRelic]`

## 资源路径
- 所有资源通过 `Entry.ResPath`（`res://Molin`）访问
- 资源目录名 = `Molin.json` 的 `id`

## 依赖方向（推荐，非强制）
`MolinCode/` 内部无强制分层，但推荐：
- `Cards/`、`Relics/` 依赖 `Characters/`（卡池、遗物池）
- 所有模块依赖 `MolinCode/Entry.cs`（ResPath、Logger）