# 命名与编码规范

## 命名
- 类、方法、属性：PascalCase
- 私有字段：_camelCase
- 局部变量、参数：camelCase
- 接口：I 前缀
- 异步方法：Async 后缀
- 常量：PascalCase

## 引用
- 游戏 API 通过 `Sts2DataDir` 下的 DLL 引用
- 模组框架通过 NuGet 包 `STS2.RitsuLib` 引用
- 不新增第三方依赖，除非两人同意

## 目录职责
- `Sts2ModCode/Entry.cs` — Mod 入口，全局常量和日志
- `Sts2ModCode/Cards/` — 卡牌类
- `Sts2ModCode/Characters/` — 角色、卡池、遗物池、药水池
- `Sts2ModCode/Relics/` — 遗物类
- `Sts2Mod/` — Godot 资源

## 禁止
- 不使用 sed 全局替换标识符
- 不修改 `Sts2Mod/` 原始资源文件（要改用副本）
- 不一次性重构全项目