```markdown
# AI 指令

你正在协助开发《杀戮尖塔2》模组。你可以直接读写文件、运行命令。以下是你必须遵守的规则。

## 项目

- 仓库根目录：当前工作目录
- GitHub仓库：`https://github.com/hwjsldgj/sts2-mod`
- 语言：C# 13 / .NET 9.0
- 引擎：Godot 4.5.1 Mono
- 框架：RitsuLib 0.6.3
- 游戏 API：`MegaCrit.Sts2.*`
- 框架 API：`STS2RitsuLib.*`

## 开工前必做

1. 运行 `git status --short --branch`，确认当前分支
2. **判断是否需要切分支**：仅可能破坏性改动、或与合伙人同时改同一区域时切分支；日常改动直接在 `main` 上做。
   ```
   git checkout -b feat/<简短描述>
   ```
3. 阅读参照文件（至少读一个同类）：
   - 卡牌：`MolinCode/Cards/MolinStrike.cs`
   - 遗物：`MolinCode/Relics/MolinRelic.cs`
   - 角色：`MolinCode/Characters/MolinCharacter.cs`

**不要凭记忆写 RitsuLib API。不确定时先读现有文件或问用户。**

## 文件放哪

| 内容 | 路径 |
|---|---|
| 卡牌 | `MolinCode/Cards/<类名>.cs` |
| 遗物 | `MolinCode/Relics/<类名>.cs` |
| 角色/卡池/遗物池/药水池 | `MolinCode/Characters/<类名>.cs` |
| 卡牌图片 | `Molin/images/cards/<类名>.png` |
| 遗物图片 | `Molin/images/relics/<类名>.png` |
| 中文文本 | `Molin/localization/zhs/*.json` |
| 英文文本 | `Molin/localization/eng/*.json` |

## 基类

| 内容 | 继承 |
|---|---|
| 卡牌 | `ModCardTemplate` |
| 遗物 | `ModRelicTemplate` |
| 角色 | `ModCharacterTemplate<MolinCardPool, MolinRelicPool, MolinPotionPool>` |
| 卡池 | `TypeListCardPoolModel` |
| 遗物池 | `TypeListRelicPoolModel` |
| 药水池 | `TypeListPotionPoolModel` |

## 必须加的标签

```csharp
[RegisterCard(typeof(MolinCardPool))]
[RegisterRelic(typeof(MolinRelicPool))]
[RegisterCharacter]
[RegisterCharacterStarterCard(typeof(MolinCharacter), 数量)]
[RegisterCharacterStarterRelic(typeof(MolinCharacter))]
```

## 资源路径

固定用 `Entry.ResPath` 拼接（值为 `res://Molin`）。

```csharp
public override CardAssetProfile AssetProfile => new(
    PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");
```

## 本地化 key 命名

- 卡牌：`MOLIN_CARD_<ID大写>.title` / `.description` / `.smartDescription`
- 遗物：`MOLIN_RELIC_<ID大写>.title` / `.description` / `.flavor`
- 角色：`MOLIN_CHARACTER_<ID大写>.<字段名>`，字段名见现有 `characters.json`
- 远古对话：`<远古名>.talk.MOLIN_CHARACTER_<ID大写>.<序号>.<char|next|ancient>`
  - 远古名例：`NEOW`、`DARV`
  - 序号例：`0-0`、`0-1`

中英文文件名相同，分别放在 `localization/eng/` 和 `localization/zhs/`。

**本地化 key 由 RitsuLib 根据"模组ID + 内容类型 + 类名"自动生成**，所以会出现 `MOLIN_CARD_MOLIN_DEFEND` 这样的双重前缀（模组名 `Molin` + 类名 `MolinDefend`）。这是正常的，不要手动改短。

## 命名

- 类/方法/属性/常量：PascalCase
- 私有字段：`_camelCase`
- 局部变量/参数：camelCase
- 异步方法：`Async` 后缀
- 接口：`I` 前缀

---

## 每次任务的交付物

写一张新卡牌，必须完成：

1. 创建 `MolinCode/Cards/<类名>.cs`
2. 追加 `Molin/localization/zhs/cards.json` 条目
3. 追加 `Molin/localization/eng/cards.json` 条目
4. 更新 `docs/features.md` 的"卡牌"段，追加一行
5. 报告用户需要准备 `Molin/images/cards/<类名>.png`（你不能生成图片）

写一个新遗物，必须完成：

1. 创建 `MolinCode/Relics/<类名>.cs`
2. 追加 `Molin/localization/zhs/relics.json` 条目
3. 追加 `Molin/localization/eng/relics.json` 条目
4. 更新 `docs/features.md` 的"遗物"段，追加一行
5. 报告用户需要准备 `Molin/images/relics/<类名>.png`

**缺任何一项，不算完成。**

---

## 文档同步

| 改动 | 必须更新 |
|---|---|
| 新增卡牌/遗物/角色 | `docs/features.md` 对应段 |
| 新增图片 | `docs/assets.md` 的"图片"段 |
| 新增本地化文件 | `docs/assets.md` 的"本地化文件"段 |
| 改动目录结构 | `ARCHITECTURE.md` |
| 改动构建方式 | `README.md` |
| 改动 API 用法 | `API_NOTES.md` |

文档只追加必要条目，不重写整篇。

---

## 编译验证

每次改完 C# 代码，**必须运行**：

```
dotnet build /p:RunPckExport=false
```

**输出必须包含"成功"，否则不允许提交。**

编译失败时：
1. 读完整错误信息
2. 修复
3. 重新编译
4. 直到通过

---

## Git 提交

### 何时提交

完成**一个独立完整的任务**后提交。不要改一行就提交。

合格的一次提交：
- 一张新卡（含 C# + 中英文 + features.md）
- 一个新遗物（含 C# + 中英文 + features.md）
- 一次 bug 修复
- 一次文档更新

不合格：
- 只写了 C# 没写本地化
- 编译不过
- 只改了注释

### 提交前必做

1. 编译通过（见上）
2. 若在分支上工作，确认分支名与改动内容相符
3. 检查改动：
   ```
   git status
   git diff --stat
   ```

### 提交命令

```
git add <具体文件>
git commit -m "类型：描述"
```

提交信息格式 `类型：描述`：

- `功能：` 新增内容
- `修复：` 修复问题
- `重构：` 重构代码
- `文档：` 文档变更
- `杂项：` 构建、配置
- `格式：` 格式调整

例：
- `功能：新增火焰斩卡牌`
- `功能：新增力量护符遗物`
- `修复：模板遗物抽牌数错误`
- `文档：更新 features.md 卡牌清单`

**不要写英文。不要写 "update" "fix bug"。**

### 推送

```
git push -u origin <当前分支名>
```

分支工作用 `git push -u origin <当前分支名>`。

---

## 输出控制

### 每次回复必须遵守

- **不要复述用户需求**
- **不要写"好的""明白了""以下是"**
- **不要写"希望能帮到你""如有问题请告诉我"**
- **不要写"总结""注意事项""下一步"**
- 报告格式固定为：

```
完成：
- <文件路径> — <一句话说明>
- <文件路径> — <一句话说明>

编译：成功 / 失败
提交：<commit hash> <提交信息>
分支：<分支名>
推送：<分支名> → origin

需要你做：
- <需要用户处理的事>
```

- 出错时只报：
  ```
  错误：<具体错误>
  位置：<文件:行号>
  原因：<一句话>
  修复：<改了什么>
  ```

### 禁止

- 输出完整文件内容（除非用户明确要求）
- 解释代码做了什么（用户会自己看）
- 复述规则

---

## API 使用规则

- **不要凭记忆或猜测写 RitsuLib / 游戏 API 的枚举值、常量、方法签名。**
- 不确定时，用以下方式核实：
  1. 读现有代码里的同类用法
  2. 读 NuGet 包里的 XML 文档：`%USERPROFILE%\.nuget\packages\sts2.ritsulib\0.6.3\lib\net9.0\*.xml`
  3. 反射 `sts2.dll` 查枚举和类型
  4. 问用户
- 如果猜了，必须在输出里标注"未验证"，让用户核实后再提交。

---

## 禁止行为

- 不要发明 RitsuLib 没有的 API
- 不要改 `Molin/` 下的图片、场景原始文件
- 不要动 `Molin.csproj`、`Molin.json`、`local.props`
- 不要一次重构多个文件
- 不要在没读参照文件时写新内容
- 不要在不切分支的情况下做破坏性改动，或与合伙人同时改同一区域
- 不要在编译不过时提交
- 不要 `git push --force`
- 不要删除别人写的内容
- 不要修改 `.gitignore`、`.gitattributes`