# AI 使用说明

## 项目背景
《杀戮尖塔2》模组，C# + Godot 4.5.1，基于 RitsuLib 框架。

## 常用基类
- 卡牌：`ModCardTemplate`（RitsuLib）
- 遗物：`ModRelicTemplate`（RitsuLib）
- 角色：`ModCharacterTemplate<卡池, 遗物池, 药水池>`
- 卡池：`TypeListCardPoolModel`
- 遗物池：`TypeListRelicPoolModel`
- 药水池：`TypeListPotionPoolModel`

## 注册标签
- 卡牌：`[RegisterCard(typeof(卡池))]`
- 遗物：`[RegisterRelic(typeof(遗物池))]`
- 角色：`[RegisterCharacter]`
- 初始卡：`[RegisterCharacterStarterCard(typeof(角色), 数量)]`
- 初始遗物：`[RegisterCharacterStarterRelic(typeof(角色))]`

## 资源路径
- 用 `Entry.ResPath`（`res://Sts2Mod`）拼接
- 资源目录名固定为 `Sts2Mod`

## AI 约束
- 只用于参考、原型、查文档、生成模板
- 不直接进仓库，必须人工审查
- 每次只给相关文件或 git diff
- 不整库喂入
- 改代码时输出 diff，不输出整个文件
- 提交 AI 代码的人必须能逐行解释
- 禁止在 CI 中调用 AI

## 提示词模板
见 prompts/ 目录。