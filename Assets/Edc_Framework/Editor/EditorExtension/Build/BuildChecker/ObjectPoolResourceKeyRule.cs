using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;


internal sealed class ObjectPoolResourceKeyRule : IEdcCheckRule
{
    public string RuleName => "对象池ResourceKey检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var poolNames = ObjectPoolRule.GetAllPoolNames();
        var poolTypes = TypeCache.GetTypesDerivedFrom<BasePool>();

        foreach (var type in poolTypes)
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
            {
                continue;
            }

            var attribute = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            if (attribute == null)
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 缺少 ResourceKeyAttribute",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            if (string.IsNullOrWhiteSpace(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 的 ResourceKey 为空",
                    EdcCheckerUtility.GetScript(type));
                continue;
            }

            if (!poolNames.Contains(attribute.Key))
            {
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.ObjectPool,
                    RuleName,
                    $"对象池类 {type.Name} 使用的 ResourceKey 不存在于 ObjectPoolSetting 中：{attribute.Key}",
                    EdcCheckerUtility.GetScript(type));
            }
        }
    }
}

