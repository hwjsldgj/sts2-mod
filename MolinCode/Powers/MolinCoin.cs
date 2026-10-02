using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Molin.Powers;

// RegisterPower 会把能力注册进 RitsuLib。
// 能力没有卡池，直接挂在生物身上即可。
[RegisterPower]
public sealed class MolinCoin : ModPowerTemplate
{
    // 只有层数等于这个值时才结算，也就是硬币的最后一轮。
    private const int SettleAmount = 1;

    // 硬币的私有数据：本能力存在期间累计的攻击牌伤害。
    // 用内部数据而不是 DynamicVars，因为它不需要显示、也不需要参与克隆。
    private sealed class CoinData
    {
        public decimal AccumulatedDamage;
    }

    // 增益（绿色）。
    public override PowerType Type => PowerType.Buff;

    // 可叠加，图标上显示层数。
    public override PowerStackType StackType => PowerStackType.Counter;

    // 能力图标。文件名对应 Molin/images/powers/MolinCoin.png。
    public override PowerAssetProfile AssetProfile => new(
        // 小图标（原版 64x64）。
        IconPath: $"{Entry.ResPath}/images/powers/{GetType().Name}.png",
        // 大图标（悬浮提示用）。
        BigIconPath: $"{Entry.ResPath}/images/powers/{GetType().Name}.png");

    // 能力每次被应用时初始化累计值。
    protected override object InitInternalData() => new CoinData();

    // 累计自己用攻击牌造成的伤害。
    // AfterAttack 对战斗里所有生物的攻击都会触发，所以要过滤所有者、出牌者和牌型。
    public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        var cardPlay = command.CardPlay;
        if (cardPlay is null || cardPlay.Card.Type != CardType.Attack || cardPlay.Player.Creature != Owner)
        {
            return Task.CompletedTask;
        }

        var data = GetInternalData<CoinData>();

        foreach (var hit in command.Results)
        {
            foreach (var result in hit)
            {
                data.AccumulatedDamage += (decimal)result.TotalDamage;
            }
        }

        return Task.CompletedTask;
    }

    // 敌人回合结束即一轮结束。
    // 层数为 1 时，把累计伤害打给所有敌人；否则只累计不结算。
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy || Amount != SettleAmount || CombatState is not { } combatState)
        {
            return;
        }

        var data = GetInternalData<CoinData>();
        decimal damage = data.AccumulatedDamage;
        data.AccumulatedDamage = 0m;

        if (damage <= 0m)
        {
            return;
        }

        // 伤害来源是能力，用 Unpowered，避免再吃一次力量之类的加成。
        await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, damage, ValueProp.Unpowered, Owner);
    }

    // 回合开始时层数 -1。
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner)
        {
            return;
        }

        await PowerCmd.Decrement(this);
    }
}
