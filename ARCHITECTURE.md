# 架构说明

## 分层
- Core：基础设施，无业务依赖
- Adapters：外部知识库适配，唯一允许直接引用外部库的层
- Cards / Relics / Powers / Events / UI：业务层，依赖 Core 和 Adapters
- Utils：通用工具，无业务依赖

## 依赖方向
业务层 → Adapters → Core
业务层 → Core
禁止反向依赖
禁止循环依赖

## Adapter 层职责
- 封装外部知识库调用
- 提供统一接口 IKnowledgeBase
- 业务代码只依赖接口，不依赖具体实现

## 模块划分
- src/Core/
- src/Adapters/
- src/Cards/
- src/Relics/
- src/Powers/
- src/Events/
- src/UI/
- src/Utils/