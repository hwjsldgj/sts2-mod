using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Molin.Characters;
using Molin.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Molin.Cards;

// RegisterCard 会把这张牌交给 RitsuLib 自动注册，并放进 Molin 角色的卡池。
[RegisterCard(typeof(MolinCardPool))]
public sealed class Cb : ModCardTemplate
{
    // 基础耗能。
    private const int BaseEnergyCost = 1;
    // 卡牌类型。
    private const CardType CardKind = CardType.Attack;
    // 卡牌稀有度。
    private const CardRarity CardRarityValue = CardRarity.Common;
    // 目标类型（AnyEnemy 表示任意敌人）。
    private const TargetType CardTarget = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示。
    private const bool ShowInCardLibrary = true;
    // 基础伤害。
    private const decimal BaseDamage = 8m;
    // 硬币被设成的层数。
    private const int CoinAmount = 1;

    // 卡图资源，文件名对应 Molin/images/cards/Cb.png（需要你自己准备）。
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");

    // 卡牌基础数值。
    // DamageVar 对上文本里的 {Damage:diff()}；能力层数变量命名成 "Power"，对上 {Power:diff()}。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(BaseDamage, ValueProp.Move),
        ModCardVars.Power<MolinCoin>("Power", CoinAmount)
    ];

    public Cb() : base(BaseEnergyCost, CardKind, CardRarityValue, CardTarget, ShowInCardLibrary)
    {
    }

    // 打出时的效果：先对目标造成伤害，再处理硬币。
    // 硬币不存在、或层数已经等于 1 时跳过，避免触发一次没意义的层数变化。
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        var ownerCreature = Owner.Creature;
        var coin = ownerCreature.Powers.OfType<MolinCoin>().FirstOrDefault();

        if (coin is null || coin.Amount == CoinAmount)
        {
            return;
        }

        // 减少的层数就是要抽的牌数，先算出来再改层数。
        int reduced = coin.Amount - CoinAmount;

        // 用 PowerCmd.ModifyAmount 走正常的能力层数流程，参数是增量，可以是负数。
        await PowerCmd.ModifyAmount(choiceContext, coin, -reduced, ownerCreature, this, false);

        // fromHandDraw 传 false：这不是回合开始的起手抽牌。
        await CardPileCmd.Draw(choiceContext, reduced, Owner, false);
    }

    // 升级后的效果逻辑，留给你自己填。
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}
