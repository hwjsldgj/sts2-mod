# 协作流程

## 角色
- 方向负责人：hwjsldgj
- 技术实现：YGG-sudo
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