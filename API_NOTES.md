# 接口速查

## 常用基类（RitsuLib）

| 内容 | 基类 |
|---|---|
| 卡牌 | `ModCardTemplate` |
| 遗物 | `ModRelicTemplate` |
| 能力 | `ModPowerTemplate` |
| 角色 | `ModCharacterTemplate<卡池, 遗物池, 药水池>` |
| 卡池 | `TypeListCardPoolModel` |
| 遗物池 | `TypeListRelicPoolModel` |
| 药水池 | `TypeListPotionPoolModel` |

## 自动注册标签

| 用途 | 标签 |
|---|---|
| 卡牌 | `[RegisterCard(typeof(卡池))]` |
| 遗物 | `[RegisterRelic(typeof(遗物池))]` |
| 能力 | `[RegisterPower]`（无参数，能力没有卡池） |
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

`FromCard(卡牌, CardPlay)` 是 0.111.0 的游戏程序集 `sts2.dll` 才有的重载；编译期引用旧版 `sts2.dll` 时只传 `this` 会报 `CS1501`，编译期引用新版而运行期加载旧版时会抛 `MissingMethodException`。

```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this, cardPlay)
    .Targeting(cardPlay.Target)
    .Execute(choiceContext);
```

## 遗物钩子（游戏 API 0.111.0）

遗物效果钩子来自游戏基类 `MegaCrit.Sts2.Core.Models.AbstractModel`，不是 RitsuLib。
遗物自身通过 `RelicModel.Owner`（`Player`）取所属玩家，再用 `Owner.Creature` 取生物。

```csharp
// 任何生物死亡后触发，含玩家自己，需自行判断是否敌人。
public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)

// 任何生物可能受到伤害后触发，伤害为 0 时也会触发。
public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
```

- `Creature.IsPrimaryEnemy` / `Creature.IsSecondaryEnemy` 判断敌人（语义未在游戏内验证，改动前先在游戏里确认）
- `DamageResult.UnblockedDamage` 为未格挡伤害，`BlockedDamage` / `TotalDamage` 同理

## 生命值指令（游戏 API 0.111.0）

| 方法 | 说明 |
|---|---|
| `CreatureCmd.GainMaxHp(Creature, decimal)` | 只提升最大生命值，不动当前生命值 |
| `CreatureCmd.LoseMaxHp(PlayerChoiceContext, Creature, decimal, bool isFromCard)` | 降低最大生命值 |
| `CreatureCmd.Heal(Creature, decimal, bool playAnim)` | 回复当前生命值 |
| `CreatureCmd.SetCurrentHp(Creature, decimal)` | 直接设定当前生命值 |
| `CreatureCmd.SetMaxHp(Creature, decimal)` | 直接设定最大生命值 |
| `CreatureCmd.SetMaxAndCurrentHp(Creature, decimal)` | 同时设定最大与当前生命值 |

`Creature.CurrentHp` / `Creature.MaxHp` 可读当前值与上限。

格挡同理：`CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay)` 需要带上 `cardPlay`。

## 能力（v0.111.0）

基类 `ModPowerTemplate`，标签 `[RegisterPower]`（无参数）。

`PowerModel` 有两个抽象属性必须实现，另有内部数据机制：

| 成员 | 说明 |
|---|---|
| `public override PowerType Type` | `Buff` / `Debuff` / `None` |
| `public override PowerStackType StackType` | `Counter`（显示层数）/ `Single` / `None` |
| `protected override object InitInternalData()` | 返回私有数据对象，克隆时重置 |
| `protected T GetInternalData<T>()` | 取回上面的对象 |
| `public override PowerAssetProfile AssetProfile` | `new(IconPath, BigIconPath)` |

`PowerType`、`PowerStackType` 在 `MegaCrit.Sts2.Core.Entities.Powers`；
`PowerInstanceType` 同理（`None` / `Instanced` / `InstancedPerApplier`）。

常用钩子（都来自 `AbstractModel`，签名与遗物一致）：

```csharp
// 任何生物攻击后都会触发，需自行过滤所有者与牌型
public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
// AttackCommand.CardPlay 为 CardPlay（Card / Player / Target），Results 为 IEnumerable<List<DamageResult>>
// 需要 await 的钩子写成 async Task，不需要的可以直接返回 Task.CompletedTask

// 某一方回合结束；CombatSide 取值为 None / Player / Enemy
public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)

// 回合开始（每个玩家各触发一次）
public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
```

层数与指令：

| 方法 | 说明 |
|---|---|
| `PowerModel.Amount` | 当前层数（`int`） |
| `PowerCmd.Decrement(PowerModel)` | 层数 -1，降到 0 时移除 |
| `PowerModel.Owner` | 能力所在生物（`Creature`） |
| `PowerModel.CombatState` | 当前战斗（`ICombatState`），`HittableEnemies` 是可打的敌人 |

直接造成伤害用 `CreatureCmd.Damage`，不需要构造攻击指令：

```csharp
await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, damage, ValueProp.Unpowered, Owner);
```

`ValueProp` 的含义见 `sts2.xml`：`Unblockable` = 类似中毒的生命流失，`Unpowered` = 遗物 / 药水 / 能力造成的伤害，
`Move` = 攻击牌与敌人攻击的伤害。

能力本地化 key 是 `MOLIN_POWER_<类名大写>`，写在 `powers` 表（`Molin/localization/<语言>/powers.json`），
常用字段 `{ENTRY}.title` / `{ENTRY}.description`。

## 参考

- RitsuLib 仓库：https://github.com/BAKAOLC/STS2-RitsuLib
- 教程站：https://tutorials.sts2modding.com/