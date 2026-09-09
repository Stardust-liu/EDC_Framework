using System;
using System.Collections.Generic;
using System.Linq;
using ArchiveData;
using UnityEditor;

internal sealed class ArchiveKeyRule : IEdcCheckRule
{
    public string RuleName => "存档标签检查";

    public IEnumerable<EdcCheckResult> Check(EdcCheckContext context)
    {
        var archiveTypes = TypeCache.GetTypesDerivedFrom<BaseGameArchive>()
            .Where(type => !type.IsAbstract && !type.ContainsGenericParameters)
            .Select(type => new
            {
                Type = type,
                Attribute = Attribute.GetCustomAttribute(type, typeof(ArchiveKeyAttribute), false) as ArchiveKeyAttribute,
            })
            .Where(item => item.Attribute != null && !string.IsNullOrWhiteSpace(item.Attribute.Key));

        foreach (var domainGroup in archiveTypes.GroupBy(item => item.Attribute.Domain))
        {
            foreach (var duplicateGroup in domainGroup
                         .GroupBy(item => item.Attribute.Key, StringComparer.OrdinalIgnoreCase)
                         .Where(group => group.Count() > 1))
            {
                var first = duplicateGroup.First();
                var typeNames = string.Join("、", duplicateGroup.Select(item => item.Type.FullName));
                yield return new EdcCheckResult(
                    EdcCheckSeverity.Error,
                    EdcCheckCategory.Archive,
                    RuleName,
                    $"{domainGroup.Key} 存档文件名重复：{duplicateGroup.Key}。涉及类型：{typeNames}",
                    EdcCheckerUtility.GetScript(first.Type));
            }
        }
    }
}
