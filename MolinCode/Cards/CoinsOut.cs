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
// 想要开局就能抽到，再加一行 [RegisterCharacterStarterCard(typeof(MolinCharacter), 数量)]。
[RegisterCard(typeof(MolinCardPool))]
[RegisterCharacterStarterCard(typeof(MolinCharacter), 1)]
public sealed class CoinsOut : ModCardTemplate
{
    // 基础耗能。
    private const int BaseEnergyCost = 1;
    // 卡牌类型。
    private const CardType CardKind = CardType.Skill;
    // 卡牌稀有度。
    private const CardRarity CardRarityValue = CardRarity.Basic;
    // 目标类型（Self 表示自己）。
    private const TargetType CardTarget = TargetType.Self;
    // 是否在卡牌图鉴中显示。
    private const bool ShowInCardLibrary = true;
    // 获得硬币的层数。
    private const decimal CoinAmount = 1m;

    // 这张牌会给自己格挡。
    public override bool GainsBlock => true;

    // 卡图资源，文件名对应 Molin/images/cards/CoinsOut.png（需要你自己准备）。
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");

    // 卡牌基础数值。
    // BlockVar 绑定文本里的 {Block:diff()}。
    // Power<硬币能力> 有两个重载：只传数值时变量名跟着类型走，传 string 时用你给的名字。
    // 这里显式命名为 "Power"，正好对上 cards.json 里的 {Power:diff()}。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(5m, ValueProp.Move),
        ModCardVars.Power<MolinCoin>("Power", CoinAmount)
    ];

    public CoinsOut() : base(BaseEnergyCost, CardKind, CardRarityValue, CardTarget, ShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<MolinCoin>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this, false);
    }

    // 升级后的效果逻辑，留给你自己填。
    // 例：DynamicVars.Block.UpgradeValueBy(3m);
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
