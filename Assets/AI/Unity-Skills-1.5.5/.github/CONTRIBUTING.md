# Contributing | 贡献指南

Thank you for contributing to Unity-Skills!
感谢你对 Unity-Skills 的贡献！

## Workflow | 贡献流程

1. **Fork** this repository | Fork 本仓库
2. **Create branch | 创建分支**: `git checkout -b feat/your-feature`
3. **Commit changes | 提交更改**: `git commit -m "feat: add new feature"`
4. **Push branch | 推送分支**: `git push origin feat/your-feature`
5. **Create Pull Request | 创建 PR**

## Before Submitting PR | 提交 PR 前

> ⚠️ **Required | 必须完成**

- [ ] Import into Unity and verify no errors | 在 Unity 中导入并确认无报错
- [ ] Test Skills work correctly in your AI tool (Claude Code, Cursor, etc.) | 在你的 AI 工具中测试 Skill 能正常使用
- [ ] Run HTTP server and verify endpoints respond | 启动 HTTP 服务并验证接口响应正常

## Commit Message Format | 提交信息格式

```
type: description
类型: 简述

Types | 类型：
- feat: New feature | 新功能
- fix: Bug fix | 修复 Bug
- docs: Documentation | 文档更新
- chore: Build/tooling | 构建/工具变更
- refactor: Code refactoring | 代码重构
```

## Code Style | 代码规范

### C# (Unity)
- Follow Unity coding conventions | 遵循 Unity 编码规范
- PascalCase for classes and methods | 类和方法使用 PascalCase
- Comments in Chinese | 使用中文注释

### Python
- Use type annotations | 使用类型注解
- Prefer async | async 优先
- Use uv for dependencies | 使用 uv 管理依赖

## Adding New Skills | 添加新 Skill

1. Create file in `SkillsForUnity/Editor/Skills/` | 在该目录下创建新文件
2. Mark method with `[UnitySkill]` attribute | 使用特性标记方法：

```csharp
[UnitySkill("skill_name", "Skill description")]
public static object YourSkill(params)
{
    // Implementation
    return new { success = true };
}
```

3. Add corresponding `.md` doc in `skills/` | 在 skills/ 目录添加文档

## Version Update | 版本号更新

Update **6 locations** when releasing | 发布时需同步更新 6 处：

| File | Location |
|------|----------|
| `agent.md` | Line 12 version table |
| `package.json` | `"version"` field |
| `CHANGELOG.md` | Add new entry at top |
| `SkillsHttpServer.cs` | `version` variable |
| `SkillRouter.cs` | `version` variable |
| `README.md` | Module table version tag |

Verify command | 检查命令：
```bash
grep -rn "x.x.x" --include="*.cs" --include="*.json" --include="*.md" | grep -E "version|版本"
```

## Feedback | 问题反馈

- Bug reports: Use Issue template | Bug 报告请使用 Issue 模板
- Feature requests welcome | 功能建议欢迎提交 Feature Request
