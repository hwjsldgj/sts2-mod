# 命名与编码规范

## 命名

- 类、方法、属性：PascalCase
- 私有字段：`_camelCase`
- 局部变量、参数：camelCase
- 接口：`I` 前缀
- 异步方法：`Async` 后缀
- 常量：PascalCase

## 引用

- 游戏 API 通过 `Sts2DataDir` 下的 DLL 引用
- 模组框架的编译引用走本机 RitsuLib 安装目录（`local.props` 的 `RitsuLibDir`）；NuGet 包 `STS2.RitsuLib` 只用于本机部署与清单版本同步
- 不新增第三方依赖，除非两人同意

## 目录职责

- `MolinCode/Entry.cs` — Mod 入口，全局常量和日志
- `MolinCode/Cards/` — 卡牌类
- `MolinCode/Characters/` — 角色、卡池、遗物池、药水池
- `MolinCode/Powers/` — 能力类
- `MolinCode/Relics/` — 遗物类
- `Molin/` — Godot 资源

## 编码

- 所有文本文件 UTF-8，无 BOM
- 换行符 LF（由 `.gitattributes` 强制）

## AI 辅助

- 项目重度使用 AI 辅助开发，两人均如此
- AI 生成的代码可直接提交，提交者需理解其逻辑
- 涉及游戏核心行为、破坏性变更、跨模块重构时需人工复核
- 提示 AI 时优先给相关文件或 git diff，不必每次整库喂入
- 禁止在 CI 中调用 AI

## 禁止

- 不使用 sed 全局替换标识符
- 不修改 `Molin/` 下的原始资源文件
- 不一次性重构全项目