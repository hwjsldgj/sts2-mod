# 命名与编码规范

## 命名
- 类、方法、属性：PascalCase
- 私有字段：_camelCase
- 局部变量、参数：camelCase
- 接口：I 前缀
- 异步方法：Async 后缀
- 常量：PascalCase

## 文件
- 一个文件一个主类型
- 文件名 = 类名
- 命名空间 = 目录路径

## 引用
- 外部知识库调用只允许出现在 src/Adapters/
- 业务代码不得直接引用外部知识库
- 不新增第三方依赖，除非两人同意

## 目录职责
- Core：日志、配置、事件总线、工具类，无业务依赖
- Adapters：外部知识库适配，唯一允许直接引用外部库的层
- Cards / Relics / Powers / Events / UI：业务层
- Utils：通用工具，无业务依赖

## 禁止
- 不使用 sed 全局替换标识符
- 不修改 assets/ 原始文件
- 不一次性重构全项目
- 不绕过 PR 直接 push 到 main