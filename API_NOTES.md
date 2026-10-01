# 协作流程

## 角色
- 方向负责人：方向、需求、文档、AI 原型、Git 操作、提交
- 技术负责人：技术决策、手写实现、质量把关

## 分支
- main：稳定版，永远可编译
- feat/xxx：功能分支
- fix/xxx：修复分支

**开发流程：**
1. 从 main 开分支：`git checkout -b feat/xxx`
2. 写代码、提交
3. 切回 main 合并：`git checkout main && git merge feat/xxx --no-ff`
4. 推送：`git push origin main`
5. 删除分支：`git branch -d feat/xxx`

不走 PR，不需要审批。

## 提交信息
- 功能：新增功能
- 修复：修复问题
- 重构：重构代码
- 文档：文档变更
- 杂项：构建、配置、杂务
- 流水线：CI 配置
- 测试：测试相关
- 格式：格式调整

## 协作
- 合伙人改完代码通知方向负责人
- 方向负责人审查、提交、推送
- 两人不同时改同一文件

## AI 使用
- AI 代码必须人工审查
- 禁止在 CI 中调用 AI
- 提交 AI 代码的人必须能逐行解释

## 日常
- 每天开工前 `git pull`
- 每周一次互审
```

---

## 3. `API_NOTES.md` 需要填充

现在只有占位。**整个替换为**：

```markdown
# 接口速查

## 常用基类（RitsuLib）

| 内容 | 基类 |
|---|---|
| 卡牌 | `ModCardTemplate` |
| 遗物 | `ModRelicTemplate` |
| 角色 | `ModCharacterTemplate<卡池, 遗物池, 药水池>` |
| 卡池 | `TypeListCardPoolModel` |
| 遗物池 | `TypeListRelicPoolModel` |
| 药水池 | `TypeListPotionPoolModel` |

## 自动注册标签

| 用途 | 标签 |
|---|---|
| 卡牌 | `[RegisterCard(typeof(卡池))]` |
| 遗物 | `[RegisterRelic(typeof(遗物池))]` |
| 角色 | `[RegisterCharacter]` |
| 初始卡 | `[RegisterCharacterStarterCard(typeof(角色), 数量)]` |
| 初始遗物 | `[RegisterCharacterStarterRelic(typeof(角色))]` |

## 资源路径

- 用 `Entry.ResPath`（值为 `res://Sts2Mod`）拼接
- 资源目录名 = `Sts2Mod.json` 的 `id`

示例：
```csharp
public override CardAssetProfile AssetProfile => new(
    PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");
```

## 常用命名空间

| 命名空间 | 用途 |
|---|---|
| `MegaCrit.Sts2.Core.Commands` | 游戏指令（伤害、格挡、抽牌） |
| `MegaCrit.Sts2.Core.Entities.Cards` | 卡牌实体 |
| `MegaCrit.Sts2.Core.Entities.Characters` | 角色实体 |
| `MegaCrit.Sts2.Core.Entities.Players` | 玩家实体 |
| `MegaCrit.Sts2.Core.Entities.Relics` | 遗物实体 |
| `MegaCrit.Sts2.Core.Localization.DynamicVars` | 动态数值 |
| `MegaCrit.Sts2.Core.Modding` | Mod 初始化 |
| `MegaCrit.Sts2.Core.Logging` | 日志 |
| `STS2RitsuLib.Interop.AutoRegistration` | 自动注册 |
| `STS2RitsuLib.Scaffolding.Content` | 内容基类 |
| `STS2RitsuLib.Scaffolding.Characters` | 角色基类 |

## 入口模式

```csharp
[ModInitializer(nameof(Initialize))]
public partial class Entry
{
    public const string ModId = "Sts2Mod";
    public const string ResPath = $"res://{ModId}";
    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
    }
}
```

## 参考文档

- RitsuLib 仓库：https://github.com/BAKAOLC/STS2-RitsuLib
- 教程站：https://tutorials.sts2modding.com/
- 模板项目：`Sts2ModCode/` 下现有代码