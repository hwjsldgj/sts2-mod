using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Molin.Characters;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Molin.Relics;

// RegisterRelic 会把遗物注册进指定遗物池。
// RegisterCharacterStarterRelic 会把它作为 MolinCharacter 的初始遗物。
[RegisterRelic(typeof(MolinRelicPool))]
[RegisterCharacterStarterRelic(typeof(MolinCharacter))]
public sealed class MolinRelic : ModRelicTemplate
{
    // 敌人死亡时，最大生命值和当前生命值各提升的点数。
    private const decimal MaxHpGainPerKill = 6m;
    // 受到未格挡伤害时，扣除的最大生命值 = 未格挡伤害 / 该除数。
    private const decimal UnblockedDamageDivisor = 2m;
    // 最大生命值和当前生命值的下限，避免把自己扣死。
    private const decimal MinMaxHp = 1m;

    // 稀有度。
    public override RelicRarity Rarity => RelicRarity.Common;

    // 图片资源统一放在 AssetProfile 里配置。
    // 三个路径可以先指向同一张图。后续有高清图或轮廓图时再拆开。
    public override RelicAssetProfile AssetProfile => new(
        // 小图标（原版 85x85）。
        IconPath: $"{Entry.ResPath}/images/relics/{GetType().Name}.png",
        // 轮廓图标（原版 85x85）。
        IconOutlinePath: $"{Entry.ResPath}/images/relics/{GetType().Name}.png",
        // 大图标（原版 256x256）。
        BigIconPath: $"{Entry.ResPath}/images/relics/{GetType().Name}.png");

    // 有敌人死亡时，最大生命值和当前生命值各 +6。
    // AfterDeath 对任何生物死亡都会触发，所以这里只处理敌人。
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (wasRemovalPrevented || (!creature.IsPrimaryEnemy && !creature.IsSecondaryEnemy))
        {
            return;
        }

        var ownerCreature = Owner.Creature;
        decimal currentBefore = (decimal)ownerCreature.CurrentHp;

        await CreatureCmd.GainMaxHp(ownerCreature, MaxHpGainPerKill);

        // 当前生命值需要单独补足。
        // 这里按目标值反推，即使 GainMaxHp 自身也回血，也不会重复加血。
        decimal missing = currentBefore + MaxHpGainPerKill - (decimal)ownerCreature.CurrentHp;
        if (missing > 0m)
        {
            await CreatureCmd.Heal(ownerCreature, missing, true);
        }
    }

    // 受到未格挡伤害时，最大生命值降低该伤害的一半（向下取整）。
    // 最大生命值和当前生命值最低保留 1 点。
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var ownerCreature = Owner.Creature;
        if (target != ownerCreature)
        {
            return;
        }

        decimal loss = Math.Floor((decimal)result.UnblockedDamage / UnblockedDamageDivisor);

        decimal maxHpBefore = (decimal)ownerCreature.MaxHp;
        if (loss > maxHpBefore - MinMaxHp)
        {
            loss = maxHpBefore - MinMaxHp;
        }

        if (loss <= 0m)
        {
            return;
        }

        decimal currentBefore = (decimal)ownerCreature.CurrentHp;

        await CreatureCmd.LoseMaxHp(choiceContext, ownerCreature, loss, false);

        // 当前生命值同步下调，最低 1 点。
        decimal targetHp = currentBefore - loss;
        if (targetHp < MinMaxHp)
        {
            targetHp = MinMaxHp;
        }

        if ((decimal)ownerCreature.CurrentHp != targetHp)
        {
            await CreatureCmd.SetCurrentHp(ownerCreature, targetHp);
        }
    }
}
