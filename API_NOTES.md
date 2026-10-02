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

- 用 `Entry.ResPath`（值为 `res://Molin`）拼接
- 资源目录名 = `Molin.json` 的 `id`

示例：

```csharp
public override CardAssetProfile AssetProfile => new(
    PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");
```

## 常用命名空间

| 命名空间 | 用途 |
|---|---|
| `MegaCrit.Sts2.Core.Commands` | 游戏指令（伤害、格挡、抽牌） |
| `MegaCrit.Sts2.Core.GameActions.Multiplayer` | 出牌上下文 `PlayerChoiceContext` |
| `MegaCrit.Sts2.Core.Entities.Cards` | 卡牌实体 |
| `MegaCrit.Sts2.Core.Entities.Characters` | 角色实体 |
| `MegaCrit.Sts2.Core.Entities.Players` | 玩家实体 |
| `MegaCrit.Sts2.Core.Entities.Relics` | 遗物实体 |
| `MegaCrit.Sts2.Core.Localization.DynamicVars` | 动态数值 |
| `MegaCrit.Sts2.Core.Modding` | Mod 初始化 |
| `MegaCrit.Sts2.Core.Models.Cards` | 卡牌模型基类 |
| `MegaCrit.Sts2.Core.Nodes.Combat` | 战斗节点（`NCreatureVisuals`） |
| `MegaCrit.Sts2.Core.ValueProps` | 数值属性 `ValueProp` |
| `MegaCrit.Sts2.Core.Logging` | 日志 |
| `STS2RitsuLib.Interop.AutoRegistration` | 自动注册 |
| `STS2RitsuLib.Scaffolding.Content` | 内容基类 |
| `STS2RitsuLib.Scaffolding.Characters` | 角色基类 |
| `STS2RitsuLib.Scaffolding.Godot` | Godot 节点工厂 |

## 入口模式

```csharp
[ModInitializer(nameof(Initialize))]
public partial class Entry
{
    public const string ModId = "Molin";
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

## 攻击指令（游戏 API 0.111.0）

`FromCard(卡牌, CardPlay)` 是 0.111.0 的游戏程序集 `sts2.dll` 才有的重载；引用旧版 `sts2.dll` 时只传 `this` 会编译报 `CS1501`。

```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this, cardPlay)
    .Targeting(cardPlay.Target)
    .Execute(choiceContext);
```

格挡同理：`CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay)` 需要带上 `cardPlay`。

## 参考

- RitsuLib 仓库：https://github.com/BAKAOLC/STS2-RitsuLib
- 教程站：https://tutorials.sts2modding.com/