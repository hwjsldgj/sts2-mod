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

## AI 使用约定
- AI 生成的代码可直接提交，提交者需理解其逻辑，能说明用途
- 涉及游戏核心行为、破坏性变更、跨模块重构时，需人工复核
- 提示 AI 时优先给相关文件或 git diff，不必每次整库喂入
- 禁止在 CI 中调用 AI（CI 环境不接 AI 服务）