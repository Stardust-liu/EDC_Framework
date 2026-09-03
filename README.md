# EDC Framework

EDC Framework 是一个面向 Unity 项目的模块化游戏开发框架，主要用于通用业务模块、资源加载流程、UI 管理流程和编辑器工具链。框架以 Hub 作为统一访问入口，通过 IOC 容器组织框架模块与游戏模块，降低模块之间的直接依赖。

## 核心能力

- **模块管理**：基于 IOC 容器注册和获取框架模块、游戏模块，支持模块初始化、准备完成和退出清理流程。
- **资源系统**：以 Addressables 作为底层资源加载方案，通过 `ResourcesModule`、`ResourceOwner` 和业务资源管理器划分资源仓库、资源持有者和业务封装层。
- **UI 管理**：支持 View、PersistentView、Window 三类 UI 面板，封装面板创建、打开、关闭、销毁、动画完成回调和返回栈逻辑。
- **本地化配置**：支持通过配置管理器加载本地化文本、音频和资源路径数据。
- **红点系统**：提供红点树结构，用于管理 UI 提示状态和节点刷新。
- **音频模块**：封装背景音乐、音效、对话音频播放，支持资源 Key 播放和 Label 预加载。
- **场景流程**：提供框架启动、游戏模块初始化、场景切换和退出释放流程。
- **打包工具**：基于 Odin Inspector 提供项目基础信息、构建配置、平台资源模块切换和打包前框架检查。
- **平台扩展**：预留平台成就接入结构，可按渠道配置平台代码和宏定义。

## 目录结构

```text
Assets/Edc_Framework/        框架源码、框架配置、编辑器工具和通用资源
Assets/Game/Demo/            示例工程内容
Assets/Platform/             当前导入到工程中的平台资源模块
Packages/                    Unity Package Manager 配置和嵌入式包
ProjectSettings/             Unity 项目设置
PlatformModules/             平台资源模块外部存放目录
```

## 环境依赖

- Unity 2022.3.15f1c1
- Addressables
- UniTask
- Odin Inspector
- DOTween
- xNode
- HybridCLR
- Steamworks.NET（平台功能示例）

## 文档

完整设计文档：

[EDC Framework Documentation](https://qazzxnq862.feishu.cn/wiki/space/7323485762769534979?ccm_open_type=lark_wiki_spaceLink&open_tab_from=wiki_home)


## License

This project is licensed under the MIT License.
