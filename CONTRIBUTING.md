# 协作流程

## 角色

- hwjsldgj：方向、需求、规范、文档、仓库管理、AI 原型
- YGG-sudo：主要代码实现

## 仓库

- GitHub 仓库：https://github.com/hwjsldgj/sts2-mod
- 双方通过 GitHub 拉取、推送
- YGG-sudo 拥有 Write 权限

## 分支

- main：稳定版，永远可编译
- feat/xxx：功能分支
- fix/xxx：修复分支

**开发流程：**

1. 从 main 开分支：`git checkout -b feat/xxx`
2. 写代码、提交
3. 推送分支：`git push -u origin feat/xxx`
4. 在 GitHub 开 PR（标题用中文 `类型：描述`），合并后删除分支：`git branch -d feat/xxx`

**有风险的改动**（构建与工程文件、`Molin.json`、依赖、重构、跨模块、影响编译或发布）必须开分支 + 走 PR，不允许直接推 `main`；低风险改动（文档、单张卡牌、单个遗物）可直接在 `main` 上做，推前先 `git pull`。任何情况都不要强制推送。

## 提交信息

- 功能：新增功能
- 修复：修复问题
- 重构：重构代码
- 文档：文档变更
- 杂项：构建、配置、杂务
- 合并：合并分支
- 流水线：CI 配置
- 测试：测试相关
- 格式：格式调整

## 协作

- 两人不同时改同一文件
- push 前先 `git pull`，避免冲突
- 涉及大改动前先在聊天里对齐

## AI 辅助

- AI 生成的代码可直接提交，提交者需理解其逻辑
- 涉及游戏核心行为、破坏性变更、跨模块重构时需人工复核
- 提示 AI 时优先给相关文件或 git diff，不必每次整库喂入
- 禁止在 CI 中调用 AI

## 日常

- 每天开工前 `git pull`
- 推前确认 `dotnet build /p:RunPckExport=false` 通过